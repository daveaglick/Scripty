using System.IO;

namespace Scripty.Core.Output
{
    public abstract class OutputFileBase : TextWriter, IOutputFileInfo
    {
        public abstract string FilePath { get; }

        public abstract BuildAction BuildAction { get; set; }

        internal OutputFileBase()
        {
        }

        public sealed override void Close() => InternalClose();

        public sealed override void Flush() => InternalFlush();

        public sealed override void Write(char value) => InternalWrite(value);

        public sealed override void Write(char[] buffer) => InternalWrite(buffer);

        public sealed override void Write(char[] buffer, int index, int count) => InternalWrite(buffer, index, count);

        public sealed override void Write(bool value) => InternalWrite(value);

        public sealed override void Write(int value) => InternalWrite(value);

        public sealed override void Write(uint value) => InternalWrite(value);

        public sealed override void Write(long value) => InternalWrite(value);

        public sealed override void Write(ulong value) => InternalWrite(value);

        public sealed override void Write(float value) => InternalWrite(value);

        public sealed override void Write(double value) => InternalWrite(value);

        public sealed override void Write(decimal value) => InternalWrite(value);

        public sealed override void Write(string value) => InternalWrite(value);

        public sealed override void Write(object value) => InternalWrite(value);

        public sealed override void Write(string format, object arg0) => InternalWrite(format, arg0);

        public sealed override void Write(string format, object arg0, object arg1) => InternalWrite(format, arg0, arg1);

        public sealed override void Write(string format, object arg0, object arg1, object arg2) => InternalWrite(format, arg0, arg1, arg2);

        public sealed override void Write(string format, params object[] arg) => InternalWrite(format, arg);

        public sealed override void WriteLine() => InternalWriteLine();

        public sealed override void WriteLine(char value) => InternalWriteLine(value);

        public sealed override void WriteLine(char[] buffer) => InternalWriteLine(buffer);

        public sealed override void WriteLine(char[] buffer, int index, int count) => InternalWriteLine(buffer, index, count);

        public sealed override void WriteLine(bool value) => InternalWriteLine(value);

        public sealed override void WriteLine(int value) => InternalWriteLine(value);

        public sealed override void WriteLine(uint value) => InternalWriteLine(value);

        public sealed override void WriteLine(long value) => InternalWriteLine(value);

        public sealed override void WriteLine(ulong value) => InternalWriteLine(value);

        public sealed override void WriteLine(float value) => InternalWriteLine(value);

        public sealed override void WriteLine(double value) => InternalWriteLine(value);

        public sealed override void WriteLine(decimal value) => InternalWriteLine(value);

        public sealed override void WriteLine(string value) => InternalWriteLine(value);

        public sealed override void WriteLine(object value) => InternalWriteLine(value);

        public sealed override void WriteLine(string format, object arg0) => InternalWriteLine(format, arg0);

        public sealed override void WriteLine(string format, object arg0, object arg1) => InternalWriteLine(format, arg0, arg1);

        public sealed override void WriteLine(string format, object arg0, object arg1, object arg2) => InternalWriteLine(format, arg0, arg1, arg2);

        public sealed override void WriteLine(string format, params object[] arg) => InternalWriteLine(format, arg);

        protected abstract void InternalClose();

        protected abstract void InternalFlush();

        protected abstract void InternalWrite(char value);

        protected abstract void InternalWrite(char[] buffer);

        protected abstract void InternalWrite(char[] buffer, int index, int count);

        protected abstract void InternalWrite(bool value);

        protected abstract void InternalWrite(int value);

        protected abstract void InternalWrite(uint value);

        protected abstract void InternalWrite(long value);

        protected abstract void InternalWrite(ulong value);

        protected abstract void InternalWrite(float value);

        protected abstract void InternalWrite(double value);

        protected abstract void InternalWrite(decimal value);

        protected abstract void InternalWrite(string value);

        protected abstract void InternalWrite(object value);

        protected abstract void InternalWrite(string format, object arg0);

        protected abstract void InternalWrite(string format, object arg0, object arg1);

        protected abstract void InternalWrite(string format, object arg0, object arg1, object arg2);

        protected abstract void InternalWrite(string format, params object[] arg);

        protected abstract void InternalWriteLine();

        protected abstract void InternalWriteLine(char value);

        protected abstract void InternalWriteLine(char[] buffer);

        protected abstract void InternalWriteLine(char[] buffer, int index, int count);

        protected abstract void InternalWriteLine(bool value);

        protected abstract void InternalWriteLine(int value);

        protected abstract void InternalWriteLine(uint value);

        protected abstract void InternalWriteLine(long value);

        protected abstract void InternalWriteLine(ulong value);

        protected abstract void InternalWriteLine(float value);

        protected abstract void InternalWriteLine(double value);

        protected abstract void InternalWriteLine(decimal value);

        protected abstract void InternalWriteLine(string value);

        protected abstract void InternalWriteLine(object value);

        protected abstract void InternalWriteLine(string format, object arg0);

        protected abstract void InternalWriteLine(string format, object arg0, object arg1);

        protected abstract void InternalWriteLine(string format, object arg0, object arg1, object arg2);

        protected abstract void InternalWriteLine(string format, params object[] arg);
    }
}
