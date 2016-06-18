using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.Core.ProjectModel
{
    public class ProjectNodes : IReadOnlyDictionary<string, ProjectNode>
    {
        private readonly ProjectTree _projectTree;
        private readonly Dictionary<string, ProjectNode> _nodes = new Dictionary<string, ProjectNode>();

        public ProjectNode Parent { get; }

        internal ProjectNodes(ProjectTree projectTree, ProjectNode parent)
        {
            _projectTree = projectTree;
            Parent = parent;
        }

        internal ProjectNode GetOrAdd(string name)
        {
            ProjectNode projectNode;
            if (!_nodes.TryGetValue(name, out projectNode))
            {
                projectNode = new ProjectNode(_projectTree, Parent, name, null);
                _nodes.Add(name, projectNode);
            }
            return projectNode;
        }

        internal void Add(string name, Microsoft.Build.Evaluation.ProjectItem projectItem) =>
            _nodes.Add(name, new ProjectNode(_projectTree, Parent, name, projectItem));

        // IDictionary<string, ProjectItem>
        public bool ContainsKey(string key) => _nodes.ContainsKey(key);
        public bool TryGetValue(string key, out ProjectNode value) => _nodes.TryGetValue(key, out value);
        public ProjectNode this[string key] => _nodes[key];
        public IEnumerable<string> Keys => _nodes.Keys;
        public IEnumerable<ProjectNode> Values => _nodes.Values;
        public IEnumerator<KeyValuePair<string, ProjectNode>> GetEnumerator() => _nodes.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => _nodes.Count;
    }
}
