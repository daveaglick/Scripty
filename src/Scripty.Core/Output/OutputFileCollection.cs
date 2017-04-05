namespace Scripty.Core.Output
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    ///     Serves as both a collection of output files and as a representation 
    ///     of a single default output file for ease of use
    /// </summary>
    /// <seealso cref="Scripty.Core.Output.OutputFile" />
    [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
    public class OutputFileCollection : OutputFile
    {
        #region "private fields"

        private readonly string _scriptFilePath;
        private string _defaultTargetFilePath;
        private readonly string _defaultTempFilePath;
        private OutputFile _defaultOutput;
        private bool _disposed;
        private bool _hasDefaultOutputBeenUsed = false;
        
        #endregion // #region "private fields"

        #region "props and const"

        public const string DEFAULT_TARGET_EXTENSION = ".cs";
        public const string DEFAULT_TEMP_EXTENSION = ".scriptytmp";

        /// <summary>
        ///     The temporary files where script output is saved to before writing to the target files.
        /// </summary>
        internal Dictionary<string, IOutputFileWriter> OutputTempFiles { get; set;} = new Dictionary<string, IOutputFileWriter>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     The default temporary file path.
        /// </summary>
        public override string TempFilePath => _defaultTempFilePath;

        /// <summary>
        ///     The default target file path
        /// </summary>
        public override string TargetFilePath => _defaultTargetFilePath;

        private OutputFile DefaultOutput
        {
            get
            {
                _hasDefaultOutputBeenUsed = true;
                return _defaultOutput ?? (_defaultOutput = this[_defaultTargetFilePath]);
            }
        }

        #endregion #region "props"

        #region "ctor"

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
            _defaultTargetFilePath = Path.ChangeExtension(scriptFilePath, DEFAULT_TARGET_EXTENSION);
            _defaultTempFilePath = BuildTempFilePath(_defaultTargetFilePath);

            RegisterOutputFile(_defaultTargetFilePath);
        }

        #endregion //#region "ctor"

        #region "dispose"

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(OutputFileCollection));
            }
            _disposed = true;

            foreach (var tmpFile in OutputTempFiles.Values)
            {
                tmpFile.Dispose();
            }
        }

        #endregion #region "dispose"

        internal List<IOutputFileWriter> GetOutputFilesForWriting()
        {
            var returnValue = new List<IOutputFileWriter>();
            foreach (var of in OutputTempFiles.Values)
            {
                of.Flush(); //the output is being retrieved for final writing
                of.Close(); // no reason to keep these open anymore
                
                if (of.KeepOutput == false)
                {
                    continue;
                }

                if (_hasDefaultOutputBeenUsed == false 
                    && of.TargetFilePath.Equals(_defaultTargetFilePath, StringComparison.OrdinalIgnoreCase))
                {
                    continue; //default file never used
                }

                if (_hasDefaultOutputBeenUsed == true
                   && of.TargetFilePath.Equals(_defaultTargetFilePath, StringComparison.OrdinalIgnoreCase)
                   && KeepOutput == false)
                {
                    continue; //default file has been used, but instructed to not retain the output
                }

                returnValue.Add(of);
            }
            return returnValue;
        }

        internal void CleanupAllTempData()
        {
            foreach (var outputFile in OutputTempFiles.Values)
            {
                if (File.Exists(outputFile.TempFilePath))
                {
                    outputFile.Flush();
                    outputFile.Close();

                    try
                    {
                        File.Delete(outputFile.TempFilePath);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(ex != null, $"Couldnt clean up temp data {ex}");
                    }
                }
            }
        }

        public OutputFile this[string targetFilePath]
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(OutputFileCollection));
                }

                if (string.IsNullOrWhiteSpace(targetFilePath))
                {
                    throw new ArgumentException("Value cannot be null or empty", nameof(targetFilePath));
                }
                
                targetFilePath = RegisterOutputFile(targetFilePath);
                return (OutputFile)OutputTempFiles[targetFilePath];
            }
        }

        private string RegisterOutputFile(string targetFilePath)
        {
            var scriptFileFolder = Path.GetDirectoryName(_scriptFilePath);
            Debug.Assert(scriptFileFolder != null, "scriptFileFolder != null");

            targetFilePath = Path.Combine(scriptFileFolder, targetFilePath);
            var tmpFilePath = BuildTempFilePath(targetFilePath);

            if (OutputTempFiles.ContainsKey(targetFilePath) == false)
            {
                Directory.CreateDirectory(scriptFileFolder);
                var outputFile = new OutputFileWriter(targetFilePath, tmpFilePath);
                OutputTempFiles.Add(targetFilePath, outputFile);
            }
            return targetFilePath;
        }

        private string BuildTempFilePath(string filePath)
        {
            return $"{filePath}.{Path.GetRandomFileName()}.{DEFAULT_TEMP_EXTENSION}";
        }

        /// <summary>
        ///     Returns
        /// </summary>
        internal ICollection<IOutputFileInfo> OutputFileInfos
        {
            get { return OutputTempFiles.Values.Cast<IOutputFileInfo>().ToList(); }
        }

        public OutputFileCollection SetFilePath(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _defaultTargetFilePath = filePath;
            _defaultOutput = null;
            return this;
        }

        public OutputFileCollection SetExtension(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            _defaultTargetFilePath = Path.ChangeExtension(_scriptFilePath, extension);
            _defaultOutput = null;
            return this;
        }

        public override BuildAction BuildAction
        {
            get { return DefaultOutput.BuildAction; }
            set { DefaultOutput.BuildAction = value; }
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

        public override bool FormatterEnabled
        {
            get { return DefaultOutput.FormatterEnabled; }
            set { DefaultOutput.FormatterEnabled = value; }
        }

        public override FormatterOptions FormatterOptions
        {
            get { return DefaultOutput.FormatterOptions; }
        }

        public override IDisposable WithIndent() => DefaultOutput.WithIndent();

        public override IDisposable WithIndent(int indentLevel) => DefaultOutput.WithIndent(indentLevel);

        public override OutputFile WriteIndent() => DefaultOutput.WriteIndent();

        public override Task WriteIndentAsync() => DefaultOutput.WriteIndentAsync();

        public override void Close() => DefaultOutput.Close();

        public override void Flush() => DefaultOutput.Flush();

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

        public override OutputFile WriteLine(string format, object arg0, object arg1, object arg2)
            => DefaultOutput.WriteLine(format, arg0, arg1, arg2);

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

        public override bool IsClosed
        {
            get { return DefaultOutput.IsClosed; }
        }
    }
}