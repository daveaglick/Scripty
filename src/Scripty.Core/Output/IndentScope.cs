using System;

namespace Scripty.Core.Output
{
    public sealed class IndentScope : IDisposable
    {
        private readonly OutputFile _outputFile;
        private readonly int _count;

        internal IndentScope(OutputFile outputFile, int count = 1)
        {
            _outputFile = outputFile;
            _count = count;
        }

        public void Dispose()
        {
            for (var i = 0; i < _count; ++i)
            {
                _outputFile.Dedent();
            }
        }
    }
}
