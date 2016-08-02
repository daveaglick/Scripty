using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.Core.Output
{
    internal class OutputFileWriter : OutputFile
    {
        private readonly StreamWriter _streamWriter;
        private int _indentLevel = 0;
        private bool _indentNextWrite = false;  // Only indent the first write after a WriteLine() call

        internal OutputFileWriter(string filePath)
        {
            _streamWriter = new StreamWriter(filePath);
            FilePath = filePath;
            BuildAction = Path.GetExtension(filePath) == ".cs" ? BuildAction.Compile : BuildAction.None;
        }

        protected override void Dispose(bool disposing)
        {
            _streamWriter.Dispose();
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
        
        public override int IndentLevel
        {
            get { return _indentLevel; }
            set
            {
                if (value >= 0)
                {
                    _indentLevel = value;
                }
            }
        }
        
        public override string IndentString { get; set; } = "    ";
        
        public override IDisposable WithIndent() => new Indent(this);
        
        public override IDisposable WithIndent(int indentLevel) => new Indent(this, indentLevel);

        private void BeforeWrite()
        {
            // See if we need to indent and reset the indent flag
            if (_indentNextWrite)
            {
                for (int c = 0; c < IndentLevel; c++)
                {
                    Write(IndentString);
                }
            }
            _indentNextWrite = false;
        }

        private async Task BeforeWriteAsync()
        {
            // See if we need to indent and reset the indent flag
            if (_indentNextWrite)
            {
                for (int c = 0; c < IndentLevel; c++)
                {
                    await WriteAsync(IndentString);
                }
            }
            _indentNextWrite = false;
        }

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
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(char[] buffer)
        {
            BeforeWrite();
            _streamWriter.Write(buffer);
            return this;
        }

        public override OutputFile Write(char[] buffer, int index, int count)
        {
            BeforeWrite();
            _streamWriter.Write(buffer, index, count);
            return this;
        }

        public override OutputFile Write(bool value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(int value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(uint value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(long value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(ulong value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(float value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(double value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(decimal value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(string value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(object value)
        {
            BeforeWrite();
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(string format, object arg0)
        {
            BeforeWrite();
            _streamWriter.Write(format, arg0);
            return this;
        }

        public override OutputFile Write(string format, object arg0, object arg1)
        {
            BeforeWrite();
            _streamWriter.Write(format, arg0, arg1);
            return this;
        }

        public override OutputFile Write(string format, object arg0, object arg1, object arg2)
        {
            BeforeWrite();
            _streamWriter.Write(format, arg0, arg1, arg2);
            return this;
        }

        public override OutputFile Write(string format, params object[] arg)
        {
            BeforeWrite();
            _streamWriter.Write(format, arg);
            return this;
        }

        public override OutputFile WriteLine()
        {
            BeforeWrite();
            _streamWriter.WriteLine();
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(char value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(char[] buffer)
        {
            BeforeWrite();
            _streamWriter.WriteLine(buffer);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(char[] buffer, int index, int count)
        {
            BeforeWrite();
            _streamWriter.WriteLine(buffer, index, count);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(bool value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(int value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(uint value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(long value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(ulong value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(float value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(double value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(decimal value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(object value)
        {
            BeforeWrite();
            _streamWriter.WriteLine(value);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0)
        {
            BeforeWrite();
            _streamWriter.WriteLine(format, arg0);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0, object arg1)
        {
            BeforeWrite();
            _streamWriter.WriteLine(format, arg0, arg1);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0, object arg1, object arg2)
        {
            BeforeWrite();
            _streamWriter.WriteLine(format, arg0, arg1, arg2);
            _indentNextWrite = true;
            return this;
        }

        public override OutputFile WriteLine(string format, params object[] arg)
        {
            BeforeWrite();
            _streamWriter.WriteLine(format, arg);
            _indentNextWrite = true;
            return this;
        }

        public override Task WriteAsync(char value) =>
            BeforeWriteAsync().ContinueWith(antecedent => _streamWriter.WriteAsync(value));

        public override Task WriteAsync(string value) =>
            BeforeWriteAsync().ContinueWith(antecedent => _streamWriter.WriteAsync(value));

        public override Task WriteAsync(char[] buffer, int index, int count) => 
            BeforeWriteAsync().ContinueWith(antecedent => _streamWriter.WriteAsync(buffer, index, count));

        public override Task WriteLineAsync(char value) => 
            _streamWriter.WriteLineAsync(value).ContinueWith(antecedent => _indentNextWrite = true);

        public override Task WriteLineAsync(string value) => 
            _streamWriter.WriteLineAsync(value).ContinueWith(antecedent => _indentNextWrite = true);

        public override Task WriteLineAsync(char[] buffer, int index, int count) => 
            _streamWriter.WriteLineAsync(buffer, index, count).ContinueWith(antecedent => _indentNextWrite = true);

        public override Task WriteLineAsync() => 
            _streamWriter.WriteLineAsync().ContinueWith(antecedent => _indentNextWrite = true);

        public override Task FlushAsync() => _streamWriter.FlushAsync();

        public override IFormatProvider FormatProvider => _streamWriter.FormatProvider;

        public override Encoding Encoding => _streamWriter.Encoding;

        public override string NewLine
        {
            get { return _streamWriter.NewLine; }
            set { _streamWriter.NewLine = value; }
        }
    }
}