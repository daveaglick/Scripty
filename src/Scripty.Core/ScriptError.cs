namespace Scripty.Core
{
    public class ScriptError
    {
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string FilePath { get; set; }
    }
}