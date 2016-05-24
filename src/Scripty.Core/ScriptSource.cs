using System;
using System.IO;

namespace Scripty.Core
{
    public class ScriptSource
    {
        public string FilePath { get; }
        public string Code { get; }
        
        public ScriptSource(string filePath, string code)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(filePath));
            }
            if (!Path.IsPathRooted(filePath))
            {
                throw new ArgumentException("The file path must be rooted", nameof(filePath));
            }
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            FilePath = filePath;
            Code = code;
        }
    }
}