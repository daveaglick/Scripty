using System;
using System.IO;
using Microsoft.CodeAnalysis.MSBuild;
using Scripty.Core.Output;
using Scripty.Core.ProjectTree;
using System.Collections.Generic;

namespace Scripty.Core
{
    public class ScriptContext : IDisposable
    {
        private List<ScriptMessage> _messages = new List<ScriptMessage>();

        internal ScriptContext(string scriptFilePath, string projectFilePath, ProjectRoot projectRoot)
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
            Project = projectRoot;
            Output = new OutputFileCollection(scriptFilePath);
            Log = new Logger(_messages);
        }

        public void Dispose()
        {
            Output.Dispose();
        }

        public ScriptContext Context => this;

        public string ScriptFilePath { get; }

        public string ProjectFilePath { get; }

        public ProjectRoot Project { get; }

        public OutputFileCollection Output { get; }

        public Logger Log { get; }

        internal ICollection<ScriptMessage> GetMessages()
        {
            return _messages;
        }
    }
}