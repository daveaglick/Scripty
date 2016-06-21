using System.Collections.Generic;
using System.Linq;

namespace Scripty.Core.ProjectTree
{
    public class ProjectNode : IReadOnlyDictionary<string, ProjectNode>
    {
        private readonly ProjectRoot _projectRoot;
        private readonly Dictionary<string, ProjectNode> _children = new Dictionary<string, ProjectNode>();

        protected ProjectNode(ProjectRoot projectRoot, string name, ProjectNode parent,
            Microsoft.Build.Evaluation.ProjectItem projectItem)
        {
            _projectRoot = projectRoot ?? (this as ProjectRoot);
            Name = name;
            Parent = parent;
            ProjectItem = projectItem;
        }

        public string Name { get; }

        public ProjectNode Parent { get; }
        
        public virtual IReadOnlyDictionary<string, ProjectNode> Children => _children;

        public Microsoft.Build.Evaluation.ProjectItem ProjectItem { get; }

        public virtual Microsoft.CodeAnalysis.Document Document
        {
            get
            {
                Microsoft.CodeAnalysis.Project analysisProject = _projectRoot.Analysis;
                if (analysisProject == null)
                {
                    return null;
                }

                List<string> folders = new List<string>();
                ProjectNode folderNode = Parent;
                while (folderNode != null && folderNode != _projectRoot)
                {
                    folders.Add(folderNode.Name);
                    folderNode = folderNode.Parent;
                }
                folders.Reverse();
                return analysisProject.Documents.FirstOrDefault(x => x.Folders.SequenceEqual(folders) && x.Name == Name);
            }
        }

        internal ProjectNode GetOrAddChild(string name)
        {
            ProjectNode projectNode;
            if (!_children.TryGetValue(name, out projectNode))
            {
                projectNode = new ProjectNode(_projectRoot, name, this, null);
                _children.Add(name, projectNode);
            }
            return projectNode;
        }

        internal void AddChild(string name, Microsoft.Build.Evaluation.ProjectItem projectItem) =>
            _children.Add(name, new ProjectNode(_projectRoot, name, this, projectItem));

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