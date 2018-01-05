using System;

namespace Scripty.Core
{
    /// <summary>
    /// Represents a script.
    /// </summary>
    public class ScriptSource
    {
        /// <summary>
        /// The path of the script file.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// The script code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptSource"/> class.
        /// </summary>
        /// <remarks>
        /// Script source needs to have file pathing in addition to the code in 
        /// order to report diagnostics correctly.
        /// </remarks>
        /// <param name="filePath">The script file path.</param>
        /// <param name="code">The code in the script file.</param>
        public ScriptSource(string filePath, string code)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(filePath));
            }

            if (!System.IO.Path.IsPathRooted(filePath))
            {
                //The PathResolver may be able to locate this root, but the caller could use it too
                throw new ArgumentException("The file path must be rooted", nameof(filePath));
            }

            FilePath = filePath;
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }
    }
}
