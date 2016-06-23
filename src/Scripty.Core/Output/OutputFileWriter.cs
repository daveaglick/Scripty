using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.Core.Output
{
    internal class OutputFileWriter : OutputFile
    {
        private readonly StreamWriter _streamWriter;
        private readonly Stack<string> _indentStack = new Stack<string>();
        private bool _atNewLine = true;

        internal OutputFileWriter(string filePath)
        {
            _streamWriter = new StreamWriter(filePath);
            FilePath = filePath;
            BuildAction = Path.GetExtension(filePath) == ".cs" ? BuildAction.Compile : BuildAction.None;
        }

        public override string FilePath { get; }

        public override BuildAction BuildAction { get; set; }

        public override IFormatProvider FormatProvider => _streamWriter.FormatProvider;

        public override Encoding Encoding => _streamWriter.Encoding;

        public override string NewLine
        {
            get { return _streamWriter.NewLine; }
            set { _streamWriter.NewLine = value; }
        }

        public override int IndentLevel => _indentStack.Count;

        public override string IndentString { get; set; } = "    ";

        public override bool IndentEnabled { get; set; } = true;

        public override OutputFile Close()
        {
            _streamWriter.Close();
            return this;
        }

        public override OutputFile Flush()
        {
            _streamWriter.Flush();
            return this;
        }

        public override OutputFile Write(char value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(char[] buffer)
        {
            IndentIfNecessary();
            _streamWriter.Write(buffer);
            return this;
        }

        public override OutputFile Write(char[] buffer, int index, int count)
        {
            IndentIfNecessary();
            _streamWriter.Write(buffer, index, count);
            return this;
        }

        public override OutputFile Write(bool value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(int value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(uint value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(long value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(ulong value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(float value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(double value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(decimal value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(string value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(object value)
        {
            IndentIfNecessary();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(string format, object arg0)
        {
            IndentIfNecessary();
            _streamWriter.Write(format, arg0);
            return this;
        }

        public override OutputFile Write(string format, object arg0, object arg1)
        {
            IndentIfNecessary();
            _streamWriter.Write(format, arg0, arg1);
            return this;
        }

        public override OutputFile Write(string format, object arg0, object arg1, object arg2)
        {
            IndentIfNecessary();
            _streamWriter.Write(format, arg0, arg1, arg2);
            return this;
        }

        public override OutputFile Write(string format, params object[] arg)
        {
            IndentIfNecessary();
            _streamWriter.Write(format, arg);
            return this;
        }

        public override OutputFile WriteLine()
        {
            _streamWriter.WriteLine();
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(char value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(char[] buffer)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(buffer);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(char[] buffer, int index, int count)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(buffer, index, count);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(bool value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(int value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(uint value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(long value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(ulong value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(float value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(double value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(decimal value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(string value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(object value)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(value);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(format, arg0);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0, object arg1)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(format, arg0, arg1);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0, object arg1, object arg2)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(format, arg0, arg1, arg2);
            _atNewLine = true;
            return this;
        }

        public override OutputFile WriteLine(string format, params object[] arg)
        {
            IndentIfNecessary();
            _streamWriter.WriteLine(format, arg);
            _atNewLine = true;
            return this;
        }

        public override Task WriteAsync(char value)
        {
            IndentIfNecessary();
            return _streamWriter.WriteAsync(value);
        }

        public override Task WriteAsync(string value)
        {
            IndentIfNecessary();
            return _streamWriter.WriteAsync(value);
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            IndentIfNecessary();
            return _streamWriter.WriteAsync(buffer, index, count);
        }

        public override Task WriteLineAsync(char value)
        {
            IndentIfNecessary();
            return _streamWriter.WriteLineAsync(value).ContinueWith((Action<Task>)(task => _atNewLine = true));
        }

        public override Task WriteLineAsync(string value)
        {
            IndentIfNecessary();
            return _streamWriter.WriteLineAsync(value).ContinueWith((Action<Task>)(task => _atNewLine = true));
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            IndentIfNecessary();
            return _streamWriter.WriteLineAsync(buffer, index, count).ContinueWith((Action<Task>)(task => _atNewLine = true));
        }

        public override Task WriteLineAsync() => _streamWriter.WriteLineAsync().ContinueWith((Action<Task>)(task => _atNewLine = true));

        public override Task FlushAsync() => _streamWriter.FlushAsync();

        public override OutputFile SetBuildAction(BuildAction buildAction)
        {
            BuildAction = buildAction;
            return this;
        }

        public override OutputFile SetNewLine(string newLine)
        {
            NewLine = newLine;
            return this;
        }

        public override OutputFile SetIndentString(string indentString)
        {
            IndentString = indentString;
            return this;
        }

        public override OutputFile SetIndentEnabled(bool indentEnabled)
        {
            IndentEnabled = indentEnabled;
            return this;
        }

        public override IndentScope Indent()
        {
            _indentStack.Push(IndentString);
            return new IndentScope(this);
        }

        public override IndentScope Indent(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "must be greater than 0");
            }

            for (var i = 0; i < count; ++i)
            {
                _indentStack.Push(IndentString);
            }
            return new IndentScope(this, count);
        }

        public override IndentScope Indent(string indentString)
        {
            _indentStack.Push(indentString);
            return new IndentScope(this);
        }

        public override OutputFile Dedent()
        {
            _indentStack.Pop();
            return this;
        }

        protected override void Dispose(bool disposing)
        {
            _streamWriter.Dispose();
        }

        private void IndentIfNecessary()
        {
            if (_atNewLine)
            {
                if (IndentEnabled)
                {
                    foreach (var indentString in _indentStack)
                    {
                        _streamWriter.Write(indentString);
                    }
                }
                _atNewLine = false;
            }
        }
    }
}