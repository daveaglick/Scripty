using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.Core.Output
{
    internal class OutputFileWriter : OutputFile
    {
        private readonly TextWriter _textWriter;
        private int _indentLevel = 0;
        private bool _indentNextWrite = false;  // Only indent the first write after a WriteLine() call

        internal OutputFileWriter(string filePath)
        {
            _textWriter = new StreamWriter(filePath);
            FilePath = filePath;
            BuildAction = Path.GetExtension(filePath) == ".cs" ? BuildAction.Compile : BuildAction.None;
        }

        // Used for testing
        internal OutputFileWriter(TextWriter textWriter)
        {
            _textWriter = textWriter;
            FilePath = null;
            BuildAction = BuildAction.None;
        }

        protected override void Dispose(bool disposing)
        {
            _textWriter.Dispose();
        }

        public override string FilePath { get; }

        public override BuildAction BuildAction { get; set; }

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
        
        public override int Indent() => IndentLevel++;

        public override int Indent(int indentLevel)
        {
            int oldIndentLevel = IndentLevel;
            IndentLevel = indentLevel;
            return oldIndentLevel;
        }

        public override int IndentLevel
        {
            get { return _indentLevel; }
            set { _indentLevel = value < 0 ? 0 : value; }
        }
        
        public override string IndentString { get; set; } = "    ";
        
        public override IDisposable WithIndent() => new Indent(this);
        
        public override IDisposable WithIndent(int indentLevel) => new Indent(this, indentLevel);

        public override void WriteIndent()
        {
            if(!string.IsNullOrEmpty(IndentString))
            {
                for (int c = 0; c < IndentLevel; c++)
                {
                    _textWriter.Write(IndentString);
                }
            }
        }

        public override async Task WriteIndentAsync()
        {
            if (!string.IsNullOrEmpty(IndentString))
            {
                for (int c = 0; c < IndentLevel; c++)
                {
                    await _textWriter.WriteAsync(IndentString);
                }
            }
        }

        private void BeforeWrite()
        {
            // See if we need to indent and reset the indent flag
            if (_indentNextWrite)
            {
                WriteIndent();
            }
            _indentNextWrite = false;
        }

        private async Task BeforeWriteAsync()
        {
            // See if we need to indent and reset the indent flag
            if (_indentNextWrite)
            {
                await WriteIndentAsync();
            }
            _indentNextWrite = false;
        }

        public override OutputFile Close()
        {
            _textWriter.Close();
            return this;
        }

        public override OutputFile Flush()
        {
            _textWriter.Flush();
            return this;
        }

        public override OutputFile Write(char value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(char[] buffer)
        {
            BeforeWrite();
            _textWriter.Write(buffer);
            return this;
        }

        public override OutputFile Write(char[] buffer, int index, int count)
        {
            BeforeWrite();
            _textWriter.Write(buffer, index, count);
            return this;
        }

        public override OutputFile Write(bool value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(int value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(uint value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(long value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(ulong value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(float value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(double value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(decimal value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(string value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(object value)
        {
            BeforeWrite();
            _textWriter.Write(value);
            return this;
        }

        public override OutputFile Write(string format, object arg0)
        {
            BeforeWrite();
            _textWriter.Write(format, arg0);
            return this;
        }

        public override OutputFile Write(string format, object arg0, object arg1)
        {
            BeforeWrite();
            _textWriter.Write(format, arg0, arg1);
            return this;
        }

        public override OutputFile Write(string format, object arg0, object arg1, object arg2)
        {
            BeforeWrite();
            _textWriter.Write(format, arg0, arg1, arg2);
            return this;
        }

        public override OutputFile Write(string format, params object[] arg)
        {
            BeforeWrite();
            _textWriter.Write(format, arg);
            return this;
        }

        public override OutputFile WriteLine()
        {
            BeforeWrite();
            _textWriter.WriteLine();
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(char value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(char[] buffer)
        {
            BeforeWrite();
            _textWriter.WriteLine(buffer);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(char[] buffer, int index, int count)
        {
            BeforeWrite();
            _textWriter.WriteLine(buffer, index, count);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(bool value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(int value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(uint value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(long value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(ulong value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(float value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(double value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(decimal value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(object value)
        {
            BeforeWrite();
            _textWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0)
        {
            BeforeWrite();
            _textWriter.WriteLine(format, arg0);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0, object arg1)
        {
            BeforeWrite();
            _textWriter.WriteLine(format, arg0, arg1);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0, object arg1, object arg2)
        {
            BeforeWrite();
            _textWriter.WriteLine(format, arg0, arg1, arg2);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string format, params object[] arg)
        {
            BeforeWrite();
            _textWriter.WriteLine(format, arg);
            _indentNextWrite = true;
            return this;
        }

        public override Task WriteAsync(char value) =>
            BeforeWriteAsync().ContinueWith(antecedent => _textWriter.WriteAsync(value));

        public override Task WriteAsync(string value) =>
            BeforeWriteAsync().ContinueWith(antecedent => _textWriter.WriteAsync(value));

        public override Task WriteAsync(char[] buffer, int index, int count) => 
            BeforeWriteAsync().ContinueWith(antecedent => _textWriter.WriteAsync(buffer, index, count));

        public override Task WriteLineAsync(char value) => 
            _textWriter.WriteLineAsync(value).ContinueWith(antecedent => _indentNextWrite = true);

        public override Task WriteLineAsync(string value) => 
            _textWriter.WriteLineAsync(value).ContinueWith(antecedent => _indentNextWrite = true);

        public override Task WriteLineAsync(char[] buffer, int index, int count) => 
            _textWriter.WriteLineAsync(buffer, index, count).ContinueWith(antecedent => _indentNextWrite = true);

        public override Task WriteLineAsync() => 
            _textWriter.WriteLineAsync().ContinueWith(antecedent => _indentNextWrite = true);

        public override Task FlushAsync() => _textWriter.FlushAsync();

        public override IFormatProvider FormatProvider => _textWriter.FormatProvider;

        public override Encoding Encoding => _textWriter.Encoding;

        public override string NewLine
        {
            get { return _textWriter.NewLine; }
            set { _textWriter.NewLine = value; }
        }
    }
}