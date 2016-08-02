namespace Scripty.Core.Output
{
    using System.Collections.Generic;
    using System.Linq;

    internal class Indention : IIndention
    {
        private readonly Stack<string> _indentionStack;

        private char _indentionCharacter;
        private byte _indentionRepeat;

        internal Indention()
        {
            _indentionStack = new Stack<string>();
        }

        string IIndention.TotalIndention => string.Concat(_indentionStack.ToArray());

        char IIndention.IndentionCharacter
        {
            get { return _indentionCharacter; }
            set { _indentionCharacter = value; }
        }

        byte IIndention.IndentionRepeat
        {
            get { return _indentionRepeat; }
            set { _indentionRepeat = value; }
        }

        IIndention IIndention.Indent()
        {
            var indent = new string(_indentionCharacter, _indentionRepeat);
            _indentionStack.Push(indent);
            return this;
        }

        public void Dispose()
        {
            if (_indentionStack.Any())
                _indentionStack.Pop();
        }
    }
}