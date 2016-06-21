using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace Scripty.Core.ProjectTree
{
    public class ProjectRoot : ProjectNode
    {
        private readonly object _projectLock = new object();
        private Microsoft.CodeAnalysis.Project _analysisProject;
        private Microsoft.Build.Evaluation.Project _buildProject;
        private bool _generatedTree;

        public ProjectRoot(string filePath)
            : base(null, string.Empty, null, null)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }

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

        public override Document Document => null;

        public override IReadOnlyDictionary<string, ProjectNode> Children
        {
            get
            {
                if(!_generatedTree)
                {
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
                            ProjectNode current = this;
                            if (segments.Length > 1 || !string.IsNullOrEmpty(segments[0]))
                            {
                                foreach (string segment in segments)
                                {
                                    current = current.GetOrAddChild(segment);
                                }
                            }
                            foreach (var item in group)
                            {
                                current.AddChild(item.FileName, item.ProjectItem);
                            }
                        }
                    }
                    _generatedTree = true;
                }
                return base.Children;
            }
        }
    }
}