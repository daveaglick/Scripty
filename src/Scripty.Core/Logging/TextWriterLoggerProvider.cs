using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scripty.Core.Logging
{
    public class TextWriterLoggerProvider : ILoggerProvider
    {
        private readonly TextWriter _textWriter;

        public TextWriterLoggerProvider(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName) => new TextWriterLogger(_textWriter);
    }
}
