using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Options;

namespace Scripty.Core.Output
{
    internal class OutputFileWriter : OutputFile
    {
        private readonly TextWriter _textWriter;
        private int _indentLevel = 0;
        private bool _indentNextWrite = false;  // Only indent the first write after a WriteLine() call
        private BuildAction _buildAction;

        internal OutputFileWriter(string filePath)
        {
            _textWriter = new StreamWriter(filePath);
            FilePath = filePath;
            _buildAction = Path.GetExtension(filePath) == ".cs" ? BuildAction.Compile : BuildAction.None;
        }

        // Used for testing
        internal OutputFileWriter(TextWriter textWriter)
        {
            _textWriter = textWriter;
            FilePath = null;
            _buildAction = BuildAction.None;
        }

        protected override void Dispose(bool disposing)
        {
            _textWriter.Dispose();
        }

        public override string FilePath { get; }

        public override BuildAction BuildAction
        {
            get { return _buildAction; }
            set { _buildAction = value; }
        }

        public override bool FormatterEnabled { get; set; }

        public override FormatterOptions FormatterOptions { get; } = new FormatterOptions();
                
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

        public override OutputFile WriteIndent()
        {
            if(!string.IsNullOrEmpty(IndentString))
            {
                for (int c = 0; c < IndentLevel; c++)
                {
                    _textWriter.Write(IndentString);
                }
            }
            return this;
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

        private OutputFile Write(Action<TextWriter> writeAction, bool indentNextWrite)
        {
            if (_indentNextWrite)
            {
                WriteIndent();
            }
            writeAction(_textWriter);
            _indentNextWrite = indentNextWrite;
            return this;
        }

        private async Task WriteAsync(Func<TextWriter, Task> writeAction, bool indentNextWrite)
        {
            if (_indentNextWrite)
            {
                await WriteIndentAsync();
            }
            await writeAction(_textWriter)
                .ContinueWith(_ => _indentNextWrite = indentNextWrite);
        }

        public override OutputFile Write(char value) => Write(x => x.Write(value), false);

        public override OutputFile Write(char[] buffer) => Write(x => x.Write(buffer), false);

        public override OutputFile Write(char[] buffer, int index, int count) => Write(x => x.Write(buffer, index, count), false);

        public override OutputFile Write(bool value) => Write(x => x.Write(value), false);

        public override OutputFile Write(int value) => Write(x => x.Write(value), false);

        public override OutputFile Write(uint value) => Write(x => x.Write(value), false);

        public override OutputFile Write(long value) => Write(x => x.Write(value), false);

        public override OutputFile Write(ulong value) => Write(x => x.Write(value), false);

        public override OutputFile Write(float value) => Write(x => x.Write(value), false);

        public override OutputFile Write(double value) => Write(x => x.Write(value), false);

        public override OutputFile Write(decimal value) => Write(x => x.Write(value), false);

        public override OutputFile Write(string value) => Write(x => x.Write(value), false);

        public override OutputFile Write(object value) => Write(x => x.Write(value), false);

        public override OutputFile Write(string format, object arg0) => Write(x => x.Write(format, arg0), false);

        public override OutputFile Write(string format, object arg0, object arg1) => Write(x => x.Write(format, arg0, arg1), false);

        public override OutputFile Write(string format, object arg0, object arg1, object arg2) => Write(x => x.Write(format, arg0, arg1, arg2), false);

        public override OutputFile Write(string format, params object[] arg) => Write(x => x.Write(format, arg), false);

        public override OutputFile WriteLine() => Write(x => x.WriteLine(), true);

        public override OutputFile WriteLine(char value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(char[] buffer) => Write(x => x.WriteLine(buffer), true);

        public override OutputFile WriteLine(char[] buffer, int index, int count) => Write(x => x.WriteLine(buffer, index, count), true);

        public override OutputFile WriteLine(bool value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(int value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(uint value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(long value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(ulong value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(float value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(double value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(decimal value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(string value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(object value) => Write(x => x.WriteLine(value), true);

        public override OutputFile WriteLine(string format, object arg0) => Write(x => x.WriteLine(format, arg0), true);

        public override OutputFile WriteLine(string format, object arg0, object arg1) => Write(x => x.WriteLine(format, arg0, arg1), true);

        public override OutputFile WriteLine(string format, object arg0, object arg1, object arg2) => Write(x => x.WriteLine(format, arg0, arg1, arg2), true);

        public override OutputFile WriteLine(string format, params object[] arg) => Write(x => x.WriteLine(format, arg), true);

        public override Task WriteAsync(char value) => WriteAsync(x => x.WriteAsync(value), false);

        public override Task WriteAsync(string value) => WriteAsync(x => x.WriteAsync(value), false);

        public override Task WriteAsync(char[] buffer, int index, int count) => WriteAsync(x => x.WriteAsync(buffer, index, count), false);

        public override Task WriteLineAsync(char value) => WriteAsync(x => x.WriteLineAsync(value), true);

        public override Task WriteLineAsync(string value) => WriteAsync(x => x.WriteLineAsync(value), true);

        public override Task WriteLineAsync(char[] buffer, int index, int count) => WriteAsync(x => x.WriteLineAsync(buffer, index, count), true);

        public override Task WriteLineAsync() => WriteAsync(x => x.WriteLineAsync(), true);

        public override Task FlushAsync() => _textWriter.FlushAsync();

        public override IFormatProvider FormatProvider => _textWriter.FormatProvider;

        public override Encoding Encoding => _textWriter.Encoding;

        public override string NewLine
        {
            get { return _textWriter.NewLine; }
            set { _textWriter.NewLine = value; }
        }

        public override string ProjectName { get; set; }

        public override void Close() => _textWriter.Close();

        public override void Flush() => _textWriter.Flush();
    }
}