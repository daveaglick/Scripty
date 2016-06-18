using System.IO;

namespace Scripty.Core.Output
{
    public abstract class OutputFile : TextWriter, IOutputFileInfo
    {
        public abstract string FilePath { get; }

        public abstract BuildAction BuildAction { get; set; }
    }
}