using Cake.Core.IO;
using Cake.Testing.Fixtures;
using System;

namespace Cake.Scripty.Tests
{
    class ScriptyFixture : ToolFixture<ScriptySettings>
    {
        public ScriptyFixture() : base("scripty.exe")
        {
        }

        public ScriptyFixture(Action<ScriptyRunner> runAction) : this()
        {
            RunAction = runAction;
        }

        private Action<ScriptyRunner> RunAction { get; set; }
        internal FilePath ProjectFilePath { get; set; } = "./Sample.csproj";

        protected override void RunTool()
        {
            var runner = new ScriptyRunner(FileSystem, Environment, ProcessRunner, Tools, ProjectFilePath, new ScriptySettings());
            RunAction?.Invoke(runner);
        }

        internal FilePath GetProjectFilePath()
        {
            return ProjectFilePath.MakeAbsolute(Environment).FullPath;
        }
    }
}
