using System;
using System.IO;
using Microsoft.CodeAnalysis.MSBuild;
using Scripty.Core.Output;
using Scripty.Core.ProjectModel;

namespace Scripty.Core
{
    public class ScriptContext : IDisposable
    {
        internal ScriptContext(string scriptFilePath, string projectFilePath, ProjectTree projectTree)
        {
            if (string.IsNullOrEmpty(scriptFilePath))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(scriptFilePath));
            }
            if (!Path.IsPathRooted(scriptFilePath))
            {
                throw new ArgumentException("The script file path must be absolute");
            }

            ScriptFilePath = scriptFilePath;
            ProjectFilePath = projectFilePath;
            ProjectTree = projectTree;
            Output = new OutputFileCollection(scriptFilePath);
        }

        public void Dispose()
        {
            Output.Dispose();
        }

        public ScriptContext Context => this;

        public string ScriptFilePath { get; }

        public string ProjectFilePath { get; }

        public ProjectTree ProjectTree { get; }

        public OutputFileCollection Output { get; }
    }
}