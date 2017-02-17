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
        private readonly string _solutionFilePath;
        private readonly Dictionary<string, string> _properties;
        private MSBuildWorkspace _workspace;
        private Microsoft.CodeAnalysis.Project _analysisProject;
        private Microsoft.Build.Evaluation.Project _buildProject;
        private bool _generatedTree;

        public ProjectRoot(string filePath)
            : this(filePath, null, null)
        {
        }

        public ProjectRoot(string projectFilePath, string solutionFilePath, IReadOnlyDictionary<string, string> properties)
            : base(null, string.Empty, null, null)
        {
            FilePath = Path.GetFullPath(projectFilePath);
            _solutionFilePath = solutionFilePath;
            _properties = new Dictionary<string, string>();

            // Convert the given properties from a read-only
            // dictionary into a read-write dictionary because
            // that is what the MSBuildWorkspace will require.
            if (properties != null)
            {
                foreach (var pair in properties)
                {
                    _properties[pair.Key] = pair.Value;
                }
            }
        }

        public string FilePath { get; }

        public MSBuildWorkspace Workspace
        {
            get
            {
                if (_workspace == null && !string.IsNullOrEmpty(FilePath))
                {
                    lock (_projectLock)
                    {
                        _workspace = MSBuildWorkspace.Create(_properties);
                    }
                }
                return _workspace;
            }
        }

        public Microsoft.CodeAnalysis.Project Analysis
        {
            get
            {
                if (_analysisProject == null && !string.IsNullOrEmpty(FilePath))
                {
                    lock (_projectLock)
                    {
                        // If we have been given a solution path, load the solution and find the project. 
                        // This ensures that if the project references the solution directory (via the 
                        // "$(SolutionDir)" property), that it will load correctly. If we only loaded the project, 
                        // then the solution directory would not be defined and the project can fail to load.
                        if (_solutionFilePath != null)
                        {
                            Solution solution = Workspace.OpenSolutionAsync(_solutionFilePath).Result;
                            _analysisProject = solution.Projects.FirstOrDefault(x => string.Equals(x.FilePath, FilePath, System.StringComparison.OrdinalIgnoreCase));

                            if (_analysisProject == null)
                            {
                                throw new System.InvalidOperationException($"Could not find the project '{FilePath}' in the solution.");
                            }
                        }
                        else
                        {
                            _analysisProject = Workspace.OpenProjectAsync(FilePath).Result;
                        }
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