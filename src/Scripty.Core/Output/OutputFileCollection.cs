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
        private string _filePath;
        private OutputFile _defaultOutput;

        private bool _disposed;

        private OutputFile DefaultOutput => _defaultOutput ?? (_defaultOutput = this[FilePath]);

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
            _filePath = Path.ChangeExtension(scriptFilePath, ".cs");
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

        public override string FilePath => _filePath;

        public OutputFileCollection SetFilePath(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _filePath = filePath;
            _defaultOutput = null;
            return this;
        }

        public OutputFileCollection SetExtension(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            _filePath = Path.ChangeExtension(_scriptFilePath, extension);
            _defaultOutput = null;
            return this;
        }

        public override BuildAction BuildAction
        {
            get { return DefaultOutput.BuildAction; }
            set { DefaultOutput.BuildAction = value; }
        }

        public override OutputFile SetBuildAction(BuildAction buildAction)
        {
            OutputFile defaultOutput = DefaultOutput;
            defaultOutput.BuildAction = buildAction;
            return defaultOutput;
        }

        public override OutputFile SetNewLine(string newLine)
        {
            OutputFile defaultOutput = DefaultOutput;
            defaultOutput.NewLine = newLine;
            return defaultOutput;
        }

        public override int Indent() => DefaultOutput.Indent();

        public override int Indent(int indentLevel) => DefaultOutput.Indent(indentLevel);

        public override int IndentLevel
        {
            get { return DefaultOutput.IndentLevel; }
            set { DefaultOutput.IndentLevel = value; }
        }

        public override string IndentString
        {
            get { return DefaultOutput.IndentString; }
            set { DefaultOutput.IndentString = value; }
        }

        public override IDisposable WithIndent() => DefaultOutput.WithIndent();

        public override IDisposable WithIndent(int indentLevel) => DefaultOutput.WithIndent(indentLevel);

        public override void WriteIndent() => DefaultOutput.WriteIndent();

        public override Task WriteIndentAsync() => DefaultOutput.WriteIndentAsync();

        public override OutputFile Close() => DefaultOutput.Close();

        public override OutputFile Flush() => DefaultOutput.Flush();

        public override OutputFile Write(char value) => DefaultOutput.Write(value);

        public override OutputFile Write(char[] buffer) => DefaultOutput.Write(buffer);

        public override OutputFile Write(char[] buffer, int index, int count) => DefaultOutput.Write(buffer, index, count);

        public override OutputFile Write(bool value) => DefaultOutput.Write(value);

        public override OutputFile Write(int value) => DefaultOutput.Write(value);

        public override OutputFile Write(uint value) => DefaultOutput.Write(value);

        public override OutputFile Write(long value) => DefaultOutput.Write(value);

        public override OutputFile Write(ulong value) => DefaultOutput.Write(value);

        public override OutputFile Write(float value) => DefaultOutput.Write(value);

        public override OutputFile Write(double value) => DefaultOutput.Write(value);

        public override OutputFile Write(decimal value) => DefaultOutput.Write(value);

        public override OutputFile Write(string value) => DefaultOutput.Write(value);

        public override OutputFile Write(object value) => DefaultOutput.Write(value);

        public override OutputFile Write(string format, object arg0) => DefaultOutput.Write(format, arg0);

        public override OutputFile Write(string format, object arg0, object arg1) => DefaultOutput.Write(format, arg0, arg1);

        public override OutputFile Write(string format, object arg0, object arg1, object arg2) => DefaultOutput.Write(format, arg0, arg1, arg2);

        public override OutputFile Write(string format, params object[] arg) => DefaultOutput.Write(format, arg);

        public override OutputFile WriteLine() => DefaultOutput.WriteLine();

        public override OutputFile WriteLine(char value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(char[] buffer) => DefaultOutput.WriteLine(buffer);

        public override OutputFile WriteLine(char[] buffer, int index, int count) => DefaultOutput.WriteLine(buffer, index, count);

        public override OutputFile WriteLine(bool value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(int value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(uint value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(long value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(ulong value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(float value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(double value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(decimal value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(string value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(object value) => DefaultOutput.WriteLine(value);

        public override OutputFile WriteLine(string format, object arg0) => DefaultOutput.WriteLine(format, arg0);

        public override OutputFile WriteLine(string format, object arg0, object arg1) => DefaultOutput.WriteLine(format, arg0, arg1);

        public override OutputFile WriteLine(string format, object arg0, object arg1, object arg2) => DefaultOutput.WriteLine(format, arg0, arg1, arg2);

        public override OutputFile WriteLine(string format, params object[] arg) => DefaultOutput.WriteLine(format, arg);

        public override Task WriteAsync(char value) => DefaultOutput.WriteAsync(value);

        public override Task WriteAsync(string value) => DefaultOutput.WriteAsync(value);

        public override Task WriteAsync(char[] buffer, int index, int count) => DefaultOutput.WriteAsync(buffer, index, count);

        public override Task WriteLineAsync(char value) => DefaultOutput.WriteLineAsync(value);

        public override Task WriteLineAsync(string value) => DefaultOutput.WriteLineAsync(value);

        public override Task WriteLineAsync(char[] buffer, int index, int count) => DefaultOutput.WriteLineAsync(buffer, index, count);

        public override Task WriteLineAsync() => DefaultOutput.WriteLineAsync();

        public override Task FlushAsync() => DefaultOutput.FlushAsync();

        public override IFormatProvider FormatProvider => DefaultOutput.FormatProvider;

        public override Encoding Encoding => DefaultOutput.Encoding;

        public override string NewLine
        {
            get { return DefaultOutput.NewLine; }
            set { DefaultOutput.NewLine = value; }
        }
    }
}