using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.Core.Output
{
    internal class OutputFileWriter : OutputFile
    {
        private readonly StreamWriter _streamWriter;

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
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(char[] buffer)
        {
            _streamWriter.Write(buffer);
            return this;
        }

        public override OutputFile Write(char[] buffer, int index, int count)
        {
            _streamWriter.Write(buffer, index, count);
            return this;
        }

        public override OutputFile Write(bool value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(int value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(uint value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(long value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(ulong value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(float value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(double value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(decimal value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(string value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(object value)
        {
            _streamWriter.Write(value);
            return this;
        }

        public override OutputFile Write(string format, object arg0)
        {
            _streamWriter.Write(format, arg0);
            return this;
        }

        public override OutputFile Write(string format, object arg0, object arg1)
        {
            _streamWriter.Write(format, arg0, arg1);
            return this;
        }

        public override OutputFile Write(string format, object arg0, object arg1, object arg2)
        {
            _streamWriter.Write(format, arg0, arg1, arg2);
            return this;
        }

        public override OutputFile Write(string format, params object[] arg)
        {
            _streamWriter.Write(format, arg);
            return this;
        }

        public override OutputFile WriteLine()
        {
            _streamWriter.WriteLine();
            return this;
        }

        public override OutputFile WriteLine(char value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(char[] buffer)
        {
            _streamWriter.WriteLine(buffer);
            return this;
        }

        public override OutputFile WriteLine(char[] buffer, int index, int count)
        {
            _streamWriter.WriteLine(buffer, index, count);
            return this;
        }

        public override OutputFile WriteLine(bool value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(int value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(uint value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(long value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(ulong value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(float value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(double value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(decimal value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(string value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(object value)
        {
            _streamWriter.WriteLine(value);
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0)
        {
            _streamWriter.WriteLine(format, arg0);
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0, object arg1)
        {
            _streamWriter.WriteLine(format, arg0, arg1);
            return this;
        }

        public override OutputFile WriteLine(string format, object arg0, object arg1, object arg2)
        {
            _streamWriter.WriteLine(format, arg0, arg1, arg2);
            return this;
        }

        public override OutputFile WriteLine(string format, params object[] arg)
        {
            _streamWriter.WriteLine(format, arg);
            return this;
        }

        public override Task WriteAsync(char value) => _streamWriter.WriteAsync(value);

        public override Task WriteAsync(string value) => _streamWriter.WriteAsync(value);

        public override Task WriteAsync(char[] buffer, int index, int count) => _streamWriter.WriteAsync(buffer, index, count);

        public override Task WriteLineAsync(char value) => _streamWriter.WriteLineAsync(value);

        public override Task WriteLineAsync(string value) => _streamWriter.WriteLineAsync(value);

        public override Task WriteLineAsync(char[] buffer, int index, int count) => _streamWriter.WriteLineAsync(buffer, index, count);

        public override Task WriteLineAsync() => _streamWriter.WriteLineAsync();

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

        public override IFormatProvider FormatProvider => _streamWriter.FormatProvider;

        public override Encoding Encoding => _streamWriter.Encoding;

        public override string NewLine
        {
            get { return _streamWriter.NewLine; }
            set { _streamWriter.NewLine = value; }
        }
    }
}