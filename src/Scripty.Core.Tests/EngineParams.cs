namespace Scripty.Core.Tests
{
    public class EngineParams
    {
        public string ScriptFile { get; set; }
        public string OutputFile { get; set; }
        public int GeneratedOutputFileCount { get; set; }
        public int ErrorCount { get; set; }
        public OutputBehavior? OutputBehavior { get; set; }
        public bool OutContentMatchesTestContent { get; set; }
    }
}