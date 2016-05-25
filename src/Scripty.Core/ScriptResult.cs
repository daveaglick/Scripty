using System;
using System.Collections.Generic;

namespace Scripty.Core
{
    public class ScriptResult
    {
        public ICollection<IOutputFileInfo> OutputFiles { get; }
        public ICollection<string> Errors { get; }

        internal ScriptResult(ICollection<IOutputFileInfo> outputFiles)
            : this(outputFiles, Array.Empty<string>())
        {
        }

        internal ScriptResult(ICollection<IOutputFileInfo> outputFiles, ICollection<string> errors)
        {
            OutputFiles = outputFiles;
            Errors = errors;
        }
    }
}