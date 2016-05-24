using System;
using System.IO;

namespace Scripty.Core
{
    public class ScriptGlobals : IDisposable
    {
        public FileWriter Output { get; }

        internal ScriptGlobals(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(filePath));
            }

            Output = new FileWriter(Path.ChangeExtension(filePath, ".cs"));
        }

        public void Dispose()
        {
            Output.Dispose();
        }
    }
}