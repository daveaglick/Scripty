namespace Scripty.Core
{
    public enum MessageType
    {
        Info,
        Warning,
        Error
    }

    public class ScriptMessage
    {
        public MessageType MessageType { get; set; }
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
    }
}