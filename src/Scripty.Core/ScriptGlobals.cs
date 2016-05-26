using System;
using System.IO;
using Microsoft.CodeAnalysis;

namespace Scripty.Core
{
    public class ScriptGlobals : IDisposable
    {
        private readonly Func<Project> _loadProject;

        public OutputFileCollection Output { get; }
        public string ProjectFilePath { get; }

        internal ScriptGlobals(string scriptFilePath, string projectFilePath, Func<Project> loadProject)
        {
            if (string.IsNullOrEmpty(scriptFilePath))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(scriptFilePath));
            }
            if (loadProject == null)
            {
                throw new ArgumentNullException(nameof(loadProject));
            }

            Output = new OutputFileCollection(scriptFilePath);
            ProjectFilePath = projectFilePath;
            _loadProject = loadProject;
        }

        public void Dispose()
        {
            Output.Dispose();
        }

        public Project Project => _loadProject();
    }
}