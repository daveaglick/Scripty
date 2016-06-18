using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripty.Core.Output
{
    public class OutputFileCollection : OutputFile
    {
        private readonly string _scriptFilePath;
        private readonly Dictionary<string, OutputFileWriter> _outputFiles
            = new Dictionary<string, OutputFileWriter>();

        private bool _disposed;

        internal OutputFileCollection(string scriptFilePath)
        {
            if (string.IsNullOrEmpty(scriptFilePath))
            { 
                throw new ArgumentException("Value cannot be null or empty", nameof(scriptFilePath));
            }
            if (!Path.IsPathRooted(scriptFilePath))
            {
                throw new ArgumentException("The file path must be rooted", nameof(scriptFilePath));
            }

            _scriptFilePath = scriptFilePath;
            FilePath = Path.ChangeExtension(scriptFilePath, ".cs");
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(OutputFileCollection));
            }
            _disposed = true;

            foreach (OutputFileWriter outputFile in _outputFiles.Values)
            {
                outputFile.Dispose();
            }
        }

        public OutputFile this[string filePath]
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(OutputFileCollection));
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("Value cannot be null or empty", nameof(filePath));
                }
                
                filePath = Path.Combine(Path.GetDirectoryName(_scriptFilePath), filePath);
                OutputFileWriter outputFile;
                if (!_outputFiles.TryGetValue(filePath, out outputFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    outputFile = new OutputFileWriter(filePath);
                    _outputFiles.Add(filePath, outputFile);
                }
                return outputFile;
            }
        }

        internal ICollection<IOutputFileInfo> OutputFiles => _outputFiles.Values.Cast<IOutputFileInfo>().ToList();

        public override string FilePath { get; }

        public override BuildAction BuildAction
        {
            get { return this[FilePath].BuildAction; }
            set { this[FilePath].BuildAction = value; }
        }

        public override void Close() => this[FilePath].Close();

        public override void Flush() => this[FilePath].Flush();

        public override void Write(char value) => this[FilePath].Write(value);

        public override void Write(char[] buffer) => this[FilePath].Write(buffer);

        public override void Write(char[] buffer, int index, int count) => this[FilePath].Write(buffer, index, count);

        public override void Write(bool value) => this[FilePath].Write(value);

        public override void Write(int value) => this[FilePath].Write(value);

        public override void Write(uint value) => this[FilePath].Write(value);

        public override void Write(long value) => this[FilePath].Write(value);

        public override void Write(ulong value) => this[FilePath].Write(value);

        public override void Write(float value) => this[FilePath].Write(value);

        public override void Write(double value) => this[FilePath].Write(value);

        public override void Write(decimal value) => this[FilePath].Write(value);

        public override void Write(string value) => this[FilePath].Write(value);

        public override void Write(object value) => this[FilePath].Write(value);

        public override void Write(string format, object arg0) => this[FilePath].Write(format, arg0);

        public override void Write(string format, object arg0, object arg1) => this[FilePath].Write(format, arg0, arg1);

        public override void Write(string format, object arg0, object arg1, object arg2) => this[FilePath].Write(format, arg0, arg1, arg2);

        public override void Write(string format, params object[] arg) => this[FilePath].Write(format, arg);

        public override void WriteLine() => this[FilePath].WriteLine();

        public override void WriteLine(char value) => this[FilePath].WriteLine(value);

        public override void WriteLine(char[] buffer) => this[FilePath].WriteLine(buffer);

        public override void WriteLine(char[] buffer, int index, int count) => this[FilePath].WriteLine(buffer, index, count);

        public override void WriteLine(bool value) => this[FilePath].WriteLine(value);

        public override void WriteLine(int value) => this[FilePath].WriteLine(value);

        public override void WriteLine(uint value) => this[FilePath].WriteLine(value);

        public override void WriteLine(long value) => this[FilePath].WriteLine(value);

        public override void WriteLine(ulong value) => this[FilePath].WriteLine(value);

        public override void WriteLine(float value) => this[FilePath].WriteLine(value);

        public override void WriteLine(double value) => this[FilePath].WriteLine(value);

        public override void WriteLine(decimal value) => this[FilePath].WriteLine(value);

        public override void WriteLine(string value) => this[FilePath].WriteLine(value);

        public override void WriteLine(object value) => this[FilePath].WriteLine(value);

        public override void WriteLine(string format, object arg0) => this[FilePath].WriteLine(format, arg0);

        public override void WriteLine(string format, object arg0, object arg1) => this[FilePath].WriteLine(format, arg0, arg1);

        public override void WriteLine(string format, object arg0, object arg1, object arg2) => this[FilePath].WriteLine(format, arg0, arg1, arg2);

        public override void WriteLine(string format, params object[] arg) => this[FilePath].WriteLine(format, arg);

        public override Task WriteAsync(char value) => this[FilePath].WriteAsync(value);

        public override Task WriteAsync(string value) => this[FilePath].WriteAsync(value);

        public override Task WriteAsync(char[] buffer, int index, int count) => this[FilePath].WriteAsync(buffer, index, count);

        public override Task WriteLineAsync(char value) => this[FilePath].WriteLineAsync(value);

        public override Task WriteLineAsync(string value) => this[FilePath].WriteLineAsync(value);

        public override Task WriteLineAsync(char[] buffer, int index, int count) => this[FilePath].WriteLineAsync(buffer, index, count);

        public override Task WriteLineAsync() => this[FilePath].WriteLineAsync();

        public override Task FlushAsync() => this[FilePath].FlushAsync();

        public override IFormatProvider FormatProvider => this[FilePath].FormatProvider;

        public override Encoding Encoding => this[FilePath].Encoding;

        public override string NewLine
        {
            get { return this[FilePath].NewLine; }
            set { this[FilePath].NewLine = value; }
        }
    }
}