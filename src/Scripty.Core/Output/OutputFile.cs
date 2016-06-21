using System.IO;

namespace Scripty.Core.Output
{
    public abstract class OutputFile : TextWriter, IOutputFileInfo
    {		
		protected IIndention Indention { get; }

        protected OutputFile()
        {
            Indention = new Indention();
        }

        public abstract string FilePath { get; }

        public abstract BuildAction BuildAction { get; set; }

        public IIndention Indent()
        {
            return Indention.Indent();
        }
    }
}