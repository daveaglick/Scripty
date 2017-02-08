namespace Scripty.Core.Output
{
    using System;
    internal interface IOutputFileWriter : IOutputFileInfo, IDisposable
    {
        void Close();
        void Flush();
    }
}