using System;
using System.Collections.Generic;

namespace Scripty.Core
{
    public class ScriptResult
    {
        public ICollection<string> OutputFiles { get; }
        public ICollection<string> Errors { get; }

        public ScriptResult(ICollection<string> outputFiles)
            : this(outputFiles, Array.Empty<string>())
        {
        }

        internal ScriptResult(ICollection<string> outputFiles, ICollection<string> errors)
        {
            OutputFiles = outputFiles;
            Errors = errors;
        }

    }
}