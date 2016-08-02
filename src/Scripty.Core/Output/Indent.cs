using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripty.Core.Output
{
    internal class Indent : IDisposable
    {
        private readonly OutputFile _outputFile;
        private readonly int _indentLevel;
        private bool _disposed = false;

        public Indent(OutputFile outputFile)
        {
            _outputFile = outputFile;
            _indentLevel = outputFile.IndentLevel++;
        }

        public Indent(OutputFile outputFile, int indentLevel)
        {
            _outputFile = outputFile;
            _indentLevel = outputFile.IndentLevel;
            outputFile.IndentLevel = indentLevel;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Indent));
            }
            _outputFile.IndentLevel = _indentLevel;
        }
    }
}