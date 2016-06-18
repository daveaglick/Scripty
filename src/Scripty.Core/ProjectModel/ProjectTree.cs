using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.MSBuild;

namespace Scripty.Core.ProjectModel
{
    public class ProjectTree : IReadOnlyDictionary<string, ProjectNode>
    {
        private readonly object _projectLock = new object();
        private Microsoft.CodeAnalysis.Project _analysisProject;
        private Microsoft.Build.Evaluation.Project _buildProject;
        private ProjectNodes _nodes;

        public string FilePath { get; }

        public ProjectTree(string filePath)
        {
            FilePath = filePath;
        }

        public Microsoft.CodeAnalysis.Project Analysis
        {
            get
            {
                if (_analysisProject == null && !string.IsNullOrEmpty(FilePath))
                {
                    lock (_projectLock)
                    {
                        MSBuildWorkspace workspace = MSBuildWorkspace.Create();
                        _analysisProject = workspace.OpenProjectAsync(FilePath).Result;
                    }
                }
                return _analysisProject;
            }
        }

        public Microsoft.Build.Evaluation.Project Build
        {
            get
            {
                if (_buildProject == null && !string.IsNullOrEmpty(FilePath))
                {
                    lock (_projectLock)
                    {
                        _buildProject = new Microsoft.Build.Evaluation.Project(FilePath);
                    }
                }
                return _buildProject;
            }
        }

        public ProjectNodes Children
        {
            get
            {
                if(_nodes == null)
                {
                    _nodes = new ProjectNodes(this, null);
                    Microsoft.Build.Evaluation.Project buildProject = Build;
                    if(buildProject != null)
                    {
                        var groups = buildProject.AllEvaluatedItems
                            .Where(x => x.ItemType == "None"
                                        || x.ItemType == "Compile"
                                        || x.ItemType == "EmbeddedResource"
                                        || x.ItemType == "Content")
                            .Where(x => !Path.IsPathRooted(x.EvaluatedInclude))
                            .Select(x => new
                            {
                                DirectoryName = Path.GetDirectoryName(x.EvaluatedInclude),
                                FileName = Path.GetFileName(x.EvaluatedInclude),
                                ProjectItem = x
                            })
                            .GroupBy(x => x.DirectoryName);
                        foreach (var group in groups)
                        {
                            string[] segments = group.Key.Split(
                                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                            ProjectNodes current = _nodes;
                            if (segments.Length > 1 || !string.IsNullOrEmpty(segments[0]))
                            {
                                foreach (string segment in segments)
                                {
                                    ProjectNode folderNode = current.GetOrAdd(segment);
                                    current = folderNode.Children;
                                }
                            }
                            foreach (var item in group)
                            {
                                current.Add(item.FileName, item.ProjectItem);
                            }
                        }
                    }
                }
                return _nodes;
            }
        }

        // IDictionary<string, ProjectItem>
        public bool ContainsKey(string key) => Children.ContainsKey(key);
        public bool TryGetValue(string key, out ProjectNode value) => Children.TryGetValue(key, out value);
        public ProjectNode this[string key] => Children[key];
        public IEnumerable<string> Keys => Children.Keys;
        public IEnumerable<ProjectNode> Values => Children.Values;
        public IEnumerator<KeyValuePair<string, ProjectNode>> GetEnumerator() => Children.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => Children.Count;
    }
}