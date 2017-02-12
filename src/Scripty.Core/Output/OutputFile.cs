using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Options;

namespace Scripty.Core.Output
{
    public abstract class OutputFile : OutputFileTextWriter, IOutputFileInfo
    {
        internal OutputFile()
        {
        }

        public abstract string FilePath { get; }

        public abstract BuildAction BuildAction { get; set; }
        
        public abstract bool FormatterEnabled { get; set; }

        public abstract FormatterOptions FormatterOptions { get; }

        public abstract string ProjectName { get; set; }
        /// <summary>
        /// Adds another level of indentation to the output content.
        /// </summary>
        /// <returns>The previous indent level.</returns>
        public abstract int Indent();

        /// <summary>
        /// Sets the specified indent level.
        /// </summary>
        /// <returns>The previous indent level.</returns>
        public abstract int Indent(int indentLevel);

        /// <summary>
        /// Gets or sets the indent level.
        /// </summary>
        /// <value>
        /// The indent level.
        /// </value>
        public abstract int IndentLevel { get; set; }

        /// <summary>
        /// Gets or sets the indent string. The default value is four spaces.
        /// </summary>
        /// <value>
        /// The indent string.
        /// </value>
        public abstract string IndentString { get; set; }

        /// <summary>
        /// Applies an indent to the output file that will be removed when the returned
        /// object is disposed.
        /// </summary>
        /// <returns>A disposable object that will reset the indent to the previous value once disposed.</returns>
        public abstract IDisposable WithIndent();

        /// <summary>
        /// Applies an indent with a specified indent level to the output file that will be
        /// removed when the returned object is disposed.
        /// </summary>
        /// <returns>A disposable object that will reset the indent to the previous value once disposed.</returns>
        public abstract IDisposable WithIndent(int indentLevel);

        /// <summary>
        /// Writes the indent string a number of times equal to the indent level .
        /// </summary>
        public abstract OutputFile WriteIndent();

        /// <summary>
        /// Writes the indent string a number of times equal to the indent level .
        /// </summary>
        public abstract Task WriteIndentAsync();

        public new abstract OutputFile Write(char value);

        public new abstract OutputFile Write(char[] buffer);

        public new abstract OutputFile Write(char[] buffer, int index, int count);

        public new abstract OutputFile Write(bool value);

        public new abstract OutputFile Write(int value);

        public new abstract OutputFile Write(uint value);

        public new abstract OutputFile Write(long value);

        public new abstract OutputFile Write(ulong value);

        public new abstract OutputFile Write(float value);

        public new abstract OutputFile Write(double value);

        public new abstract OutputFile Write(decimal value);

        public new abstract OutputFile Write(string value);

        public new abstract OutputFile Write(object value);

        public new abstract OutputFile Write(string format, object arg0);

        public new abstract OutputFile Write(string format, object arg0, object arg1);

        public new abstract OutputFile Write(string format, object arg0, object arg1, object arg2);

        public new abstract OutputFile Write(string format, params object[] arg);

        public new abstract OutputFile WriteLine();

        public new abstract OutputFile WriteLine(char value);

        public new abstract OutputFile WriteLine(char[] buffer);

        public new abstract OutputFile WriteLine(char[] buffer, int index, int count);

        public new abstract OutputFile WriteLine(bool value);

        public new abstract OutputFile WriteLine(int value);

        public new abstract OutputFile WriteLine(uint value);

        public new abstract OutputFile WriteLine(long value);

        public new abstract OutputFile WriteLine(ulong value);

        public new abstract OutputFile WriteLine(float value);

        public new abstract OutputFile WriteLine(double value);

        public new abstract OutputFile WriteLine(decimal value);

        public new abstract OutputFile WriteLine(string value);

        public new abstract OutputFile WriteLine(object value);

        public new abstract OutputFile WriteLine(string format, object arg0);

        public new abstract OutputFile WriteLine(string format, object arg0, object arg1);

        public new abstract OutputFile WriteLine(string format, object arg0, object arg1, object arg2);

        public new abstract OutputFile WriteLine(string format, params object[] arg);

        protected sealed override void TextWriterWrite(char value) => Write(value);

        protected sealed override void TextWriterWrite(char[] buffer) => Write(buffer);

        protected sealed override void TextWriterWrite(char[] buffer, int index, int count) => Write(buffer, index, count);

        protected sealed override void TextWriterWrite(bool value) => Write(value);

        protected sealed override void TextWriterWrite(int value) => Write(value);

        protected sealed override void TextWriterWrite(uint value) => Write(value);

        protected sealed override void TextWriterWrite(long value) => Write(value);

        protected sealed override void TextWriterWrite(ulong value) => Write(value);

        protected sealed override void TextWriterWrite(float value) => Write(value);

        protected sealed override void TextWriterWrite(double value) => Write(value);

        protected sealed override void TextWriterWrite(decimal value) => Write(value);

        protected sealed override void TextWriterWrite(string value) => Write(value);

        protected sealed override void TextWriterWrite(object value) => Write(value);

        protected sealed override void TextWriterWrite(string format, object arg0) => Write(format, arg0);

        protected sealed override void TextWriterWrite(string format, object arg0, object arg1) => Write(format, arg0, arg1);

        protected sealed override void TextWriterWrite(string format, object arg0, object arg1, object arg2) => Write(format, arg0, arg1, arg2);

        protected sealed override void TextWriterWrite(string format, params object[] arg) => Write(format, arg);

        protected sealed override void TextWriterWriteLine() => WriteLine();

        protected sealed override void TextWriterWriteLine(char value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(char[] buffer) => WriteLine(buffer);

        protected sealed override void TextWriterWriteLine(char[] buffer, int index, int count) => WriteLine(buffer, index, count);

        protected sealed override void TextWriterWriteLine(bool value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(int value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(uint value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(long value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(ulong value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(float value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(double value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(decimal value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(string value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(object value) => WriteLine(value);

        protected sealed override void TextWriterWriteLine(string format, object arg0) => WriteLine(format, arg0);

        protected sealed override void TextWriterWriteLine(string format, object arg0, object arg1) => WriteLine(format, arg0, arg1);

        protected sealed override void TextWriterWriteLine(string format, object arg0, object arg1, object arg2) => WriteLine(format, arg0, arg1, arg2);

        protected sealed override void TextWriterWriteLine(string format, params object[] arg) => WriteLine(format, arg);
    }
}