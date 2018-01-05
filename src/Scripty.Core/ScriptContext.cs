using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Scripty.Core
{
    public class ScriptContext
    {
        private readonly ScriptEngine _engine;

        internal ScriptContext(ScriptEngine engine, string scriptFilePath, StreamWriter output)
        {
            _engine = engine;
            Logger = engine.LoggerFactory.CreateLogger<ScriptEngine>();
            Output = output;
        }

        public ScriptContext Context => this;

        public ILogger Logger { get; }

        public string ScriptFilePath { get; }

        public string ProjectFilePath => _engine.ProjectFilePath;

        public string SolutionFilePath => _engine.SolutionFilePath;

        public Project Project => _engine.Project;

        public StreamWriter Output { get; }
    }
}
