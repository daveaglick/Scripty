using System;
using System.IO;
using Microsoft.CodeAnalysis;

namespace Scripty.Core
{
    public class ScriptGlobals : IDisposable
    {
        private readonly Func<Project> _loadProject;

        internal ScriptGlobals(string scriptFilePath, string projectFilePath, Func<Project> loadProject)
        {
            if (string.IsNullOrEmpty(scriptFilePath))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(scriptFilePath));
            }
            if (!Path.IsPathRooted(scriptFilePath))
            {
                throw new ArgumentException("The script file path must be absolute");
            }
            if (loadProject == null)
            {
                throw new ArgumentNullException(nameof(loadProject));
            }

            Output = new OutputFileCollection(scriptFilePath);
            ScriptFilePath = scriptFilePath;
            ProjectFilePath = projectFilePath;
            _loadProject = loadProject;
        }

        public void Dispose()
        {
            Output.Dispose();
        }

        public string ScriptFilePath { get; }

        public string ProjectFilePath { get; }

        public OutputFileCollection Output { get; }

        public Project Project => _loadProject();
    }
}