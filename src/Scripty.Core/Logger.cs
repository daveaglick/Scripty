using System.Collections.Generic;

namespace Scripty.Core
{
    public class Logger
    {
        private List<ScriptMessage> _messages;

        public Logger(List<ScriptMessage> messages)
        {
            _messages = messages;
        }

        public void Info(string message, int line = 0, int column = 0)
        {
            Message(MessageType.Error, message, line, column);
        }

        public void Warning(string message, int line = 0, int column = 0)
        {
            Message(MessageType.Warning, message, line, column);
        }

        public void Error(string message, int line = 0, int column = 0)
        {
            Message(MessageType.Error, message, line, column);
        }

        private void Message(MessageType type, string message, int line, int column)
        {
            _messages.Add(new ScriptMessage
            {
                MessageType = type,
                Message = message,
                Column = column,
                Line = line
            });
        }
    }
}