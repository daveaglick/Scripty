namespace Scripty.Core.Output
{
    public abstract class OutputFile : OutputFileBase
    {
        public new abstract OutputFile Close();

        public new abstract OutputFile Flush();

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

        public abstract OutputFile SetBuildAction(BuildAction buildAction);

        public abstract OutputFile SetNewLine(string newLine);

        protected sealed override void InternalClose()
        {
            Close();
        }

        protected sealed override void InternalFlush()
        {
            Flush();
        }

        protected sealed override void InternalWrite(char value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(char[] buffer)
        {
            Write(buffer);
        }

        protected sealed override void InternalWrite(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
        }

        protected sealed override void InternalWrite(bool value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(int value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(uint value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(long value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(ulong value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(float value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(double value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(decimal value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(string value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(object value)
        {
            Write(value);
        }

        protected sealed override void InternalWrite(string format, object arg0)
        {
            Write(format, arg0);
        }

        protected sealed override void InternalWrite(string format, object arg0, object arg1)
        {
            Write(format, arg0, arg1);
        }

        protected sealed override void InternalWrite(string format, object arg0, object arg1, object arg2)
        {
            Write(format, arg0, arg1, arg2);
        }

        protected sealed override void InternalWrite(string format, params object[] arg)
        {
            Write(format, arg);
        }

        protected sealed override void InternalWriteLine()
        {
            WriteLine();
        }

        protected sealed override void InternalWriteLine(char value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(char[] buffer)
        {
            WriteLine(buffer);
        }

        protected sealed override void InternalWriteLine(char[] buffer, int index, int count)
        {
            WriteLine(buffer, index, count);
        }

        protected sealed override void InternalWriteLine(bool value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(int value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(uint value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(long value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(ulong value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(float value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(double value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(decimal value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(string value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(object value)
        {
            WriteLine(value);
        }

        protected sealed override void InternalWriteLine(string format, object arg0)
        {
            WriteLine(format, arg0);
        }

        protected sealed override void InternalWriteLine(string format, object arg0, object arg1)
        {
            WriteLine(format, arg0, arg1);
        }

        protected sealed override void InternalWriteLine(string format, object arg0, object arg1, object arg2)
        {
            WriteLine(format, arg0, arg1, arg2);
        }

        protected sealed override void InternalWriteLine(string format, params object[] arg)
        {
            WriteLine(format, arg);
        }
    }
}