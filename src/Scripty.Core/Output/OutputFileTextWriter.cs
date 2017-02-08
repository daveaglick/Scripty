using System.IO;

namespace Scripty.Core.Output
{
    /// <summary>
    /// This class only serves to implement the <see cref="TextWriter"/> API so that the
    /// actual <see cref="OutputFile"/> class can provide a fluent version of the API.
    /// The two levels of abstraction are required to properly hide the base
    /// <see cref="TextWriter"/> implementation which is not fluent.
    /// </summary>
    public abstract class OutputFileTextWriter : TextWriter
    {
        internal OutputFileTextWriter()
        {
        }

        public sealed override void Write(char value) => TextWriterWrite(value);

        public sealed override void Write(char[] buffer) => TextWriterWrite(buffer);

        public sealed override void Write(char[] buffer, int index, int count) => TextWriterWrite(buffer, index, count);

        public sealed override void Write(bool value) => TextWriterWrite(value);

        public sealed override void Write(int value) => TextWriterWrite(value);

        public sealed override void Write(uint value) => TextWriterWrite(value);

        public sealed override void Write(long value) => TextWriterWrite(value);

        public sealed override void Write(ulong value) => TextWriterWrite(value);

        public sealed override void Write(float value) => TextWriterWrite(value);

        public sealed override void Write(double value) => TextWriterWrite(value);

        public sealed override void Write(decimal value) => TextWriterWrite(value);

        public sealed override void Write(string value) => TextWriterWrite(value);

        public sealed override void Write(object value) => TextWriterWrite(value);

        public sealed override void Write(string format, object arg0) => TextWriterWrite(format, arg0);

        public sealed override void Write(string format, object arg0, object arg1) => TextWriterWrite(format, arg0, arg1);

        public sealed override void Write(string format, object arg0, object arg1, object arg2) => TextWriterWrite(format, arg0, arg1, arg2);

        public sealed override void Write(string format, params object[] arg) => TextWriterWrite(format, arg);

        public sealed override void WriteLine() => TextWriterWriteLine();

        public sealed override void WriteLine(char value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(char[] buffer) => TextWriterWriteLine(buffer);

        public sealed override void WriteLine(char[] buffer, int index, int count) => TextWriterWriteLine(buffer, index, count);

        public sealed override void WriteLine(bool value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(int value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(uint value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(long value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(ulong value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(float value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(double value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(decimal value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(string value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(object value) => TextWriterWriteLine(value);

        public sealed override void WriteLine(string format, object arg0) => TextWriterWriteLine(format, arg0);

        public sealed override void WriteLine(string format, object arg0, object arg1) => TextWriterWriteLine(format, arg0, arg1);

        public sealed override void WriteLine(string format, object arg0, object arg1, object arg2) => TextWriterWriteLine(format, arg0, arg1, arg2);

        public sealed override void WriteLine(string format, params object[] arg) => TextWriterWriteLine(format, arg);
        
        protected abstract void TextWriterWrite(char value);

        protected abstract void TextWriterWrite(char[] buffer);

        protected abstract void TextWriterWrite(char[] buffer, int index, int count);

        protected abstract void TextWriterWrite(bool value);

        protected abstract void TextWriterWrite(int value);

        protected abstract void TextWriterWrite(uint value);

        protected abstract void TextWriterWrite(long value);

        protected abstract void TextWriterWrite(ulong value);

        protected abstract void TextWriterWrite(float value);

        protected abstract void TextWriterWrite(double value);

        protected abstract void TextWriterWrite(decimal value);

        protected abstract void TextWriterWrite(string value);

        protected abstract void TextWriterWrite(object value);

        protected abstract void TextWriterWrite(string format, object arg0);

        protected abstract void TextWriterWrite(string format, object arg0, object arg1);

        protected abstract void TextWriterWrite(string format, object arg0, object arg1, object arg2);

        protected abstract void TextWriterWrite(string format, params object[] arg);

        protected abstract void TextWriterWriteLine();

        protected abstract void TextWriterWriteLine(char value);

        protected abstract void TextWriterWriteLine(char[] buffer);

        protected abstract void TextWriterWriteLine(char[] buffer, int index, int count);

        protected abstract void TextWriterWriteLine(bool value);

        protected abstract void TextWriterWriteLine(int value);

        protected abstract void TextWriterWriteLine(uint value);

        protected abstract void TextWriterWriteLine(long value);

        protected abstract void TextWriterWriteLine(ulong value);

        protected abstract void TextWriterWriteLine(float value);

        protected abstract void TextWriterWriteLine(double value);

        protected abstract void TextWriterWriteLine(decimal value);

        protected abstract void TextWriterWriteLine(string value);

        protected abstract void TextWriterWriteLine(object value);

        protected abstract void TextWriterWriteLine(string format, object arg0);

        protected abstract void TextWriterWriteLine(string format, object arg0, object arg1);

        protected abstract void TextWriterWriteLine(string format, object arg0, object arg1, object arg2);

        protected abstract void TextWriterWriteLine(string format, params object[] arg);
    }
}
