using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scripty.Core.Logging
{
    internal class TextWriterLogger : ILogger
    {
        private readonly TextWriter _textWriter;

        public TextWriterLogger(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
            _textWriter.Write(formatter(state, exception));

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => new EmptyDisposable();
    }
}
