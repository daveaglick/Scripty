using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.Core
{
    public class FileWriter : TextWriter
    {
        private readonly string _filePath;
        private readonly Dictionary<string, StreamWriter> _outputs
            = new Dictionary<string, StreamWriter>();

        private bool _disposed;

        internal FileWriter(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            { 
                throw new ArgumentException("Value cannot be null or empty", nameof(filePath));
            }
            if (!Path.IsPathRooted(filePath))
            {
                throw new ArgumentException("The file path must be rooted", nameof(filePath));
            }

            _filePath = filePath;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(FileWriter));
            }
            _disposed = true;

            foreach (StreamWriter writer in _outputs.Values)
            {
                writer.Dispose();
            }
        }

        public TextWriter this[string filePath]
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(FileWriter));
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("Value cannot be null or empty", nameof(filePath));
                }

                StreamWriter writer;
                if (!_outputs.TryGetValue(filePath, out writer))
                {
                    writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(_filePath), filePath));
                    _outputs.Add(filePath, writer);
                }
                return writer;
            }
        }

        public override void Close() => this[_filePath].Close();

        public override void Flush() => this[_filePath].Flush();

        public override void Write(char value) => this[_filePath].Write(value);

        public override void Write(char[] buffer) => this[_filePath].Write(buffer);

        public override void Write(char[] buffer, int index, int count) => this[_filePath].Write(buffer, index, count);

        public override void Write(bool value) => this[_filePath].Write(value);

        public override void Write(int value) => this[_filePath].Write(value);

        public override void Write(uint value) => this[_filePath].Write(value);

        public override void Write(long value) => this[_filePath].Write(value);

        public override void Write(ulong value) => this[_filePath].Write(value);

        public override void Write(float value) => this[_filePath].Write(value);

        public override void Write(double value) => this[_filePath].Write(value);

        public override void Write(decimal value) => this[_filePath].Write(value);

        public override void Write(string value) => this[_filePath].Write(value);

        public override void Write(object value) => this[_filePath].Write(value);

        public override void Write(string format, object arg0) => this[_filePath].Write(format, arg0);

        public override void Write(string format, object arg0, object arg1) => this[_filePath].Write(format, arg0, arg1);

        public override void Write(string format, object arg0, object arg1, object arg2) => this[_filePath].Write(format, arg0, arg1, arg2);

        public override void Write(string format, params object[] arg) => this[_filePath].Write(format, arg);

        public override void WriteLine() => this[_filePath].WriteLine();

        public override void WriteLine(char value) => this[_filePath].WriteLine(value);

        public override void WriteLine(char[] buffer) => this[_filePath].WriteLine(buffer);

        public override void WriteLine(char[] buffer, int index, int count) => this[_filePath].WriteLine(buffer, index, count);

        public override void WriteLine(bool value) => this[_filePath].WriteLine(value);

        public override void WriteLine(int value) => this[_filePath].WriteLine(value);

        public override void WriteLine(uint value) => this[_filePath].WriteLine(value);

        public override void WriteLine(long value) => this[_filePath].WriteLine(value);

        public override void WriteLine(ulong value) => this[_filePath].WriteLine(value);

        public override void WriteLine(float value) => this[_filePath].WriteLine(value);

        public override void WriteLine(double value) => this[_filePath].WriteLine(value);

        public override void WriteLine(decimal value) => this[_filePath].WriteLine(value);

        public override void WriteLine(string value) => this[_filePath].WriteLine(value);

        public override void WriteLine(object value) => this[_filePath].WriteLine(value);

        public override void WriteLine(string format, object arg0) => this[_filePath].WriteLine(format, arg0);

        public override void WriteLine(string format, object arg0, object arg1) => this[_filePath].WriteLine(format, arg0, arg1);

        public override void WriteLine(string format, object arg0, object arg1, object arg2) => this[_filePath].WriteLine(format, arg0, arg1, arg2);

        public override void WriteLine(string format, params object[] arg) => this[_filePath].WriteLine(format, arg);

        public override Task WriteAsync(char value) => this[_filePath].WriteAsync(value);

        public override Task WriteAsync(string value) => this[_filePath].WriteAsync(value);

        public override Task WriteAsync(char[] buffer, int index, int count) => this[_filePath].WriteAsync(buffer, index, count);

        public override Task WriteLineAsync(char value) => this[_filePath].WriteLineAsync(value);

        public override Task WriteLineAsync(string value) => this[_filePath].WriteLineAsync(value);

        public override Task WriteLineAsync(char[] buffer, int index, int count) => this[_filePath].WriteLineAsync(buffer, index, count);

        public override Task WriteLineAsync() => this[_filePath].WriteLineAsync();

        public override Task FlushAsync() => this[_filePath].FlushAsync();

        public override IFormatProvider FormatProvider => this[_filePath].FormatProvider;

        public override Encoding Encoding => this[_filePath].Encoding;

        public override string NewLine
        {
            get { return this[_filePath].NewLine; }
            set { this[_filePath].NewLine = value; }
        }
    }
}