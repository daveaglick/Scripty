using System.Collections.Generic;
using System.Linq;

namespace Scripty.Core.ProjectModel
{
    public class ProjectNode : IReadOnlyDictionary<string, ProjectNode>
    {
        private readonly ProjectTree _projectTree;

        public ProjectNode Parent { get; }
        public ProjectNodes Children { get; }
        public string Name { get; }
        public Microsoft.Build.Evaluation.ProjectItem ProjectItem { get; }

        internal ProjectNode(ProjectTree projectTree, ProjectNode parent, string name,
            Microsoft.Build.Evaluation.ProjectItem projectItem)
        {
            _projectTree = projectTree;
            Children = new ProjectNodes(projectTree, this);
            Parent = parent;
            Name = name;
            ProjectItem = projectItem;
        }

        public Microsoft.CodeAnalysis.Document Document
        {
            get
            {
                Microsoft.CodeAnalysis.Project analysisProject = _projectTree.Analysis;
                if (analysisProject == null)
                {
                    return null;
                }

                List<string> folders = new List<string>();
                ProjectNode folderNode = Parent;
                while (folderNode != null)
                {
                    folders.Add(folderNode.Name);
                    folderNode = folderNode.Parent;
                }
                folders.Reverse();
                return analysisProject.Documents.FirstOrDefault(x => x.Folders.SequenceEqual(folders) && x.Name == Name);
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