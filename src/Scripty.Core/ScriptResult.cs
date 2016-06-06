using System;
using System.Collections.Generic;

namespace Scripty.Core
{
    public class ScriptResult
    {
        public ICollection<IOutputFileInfo> OutputFiles { get; }
        public ICollection<ScriptError> Errors { get; }

        internal ScriptResult(ICollection<IOutputFileInfo> outputFiles)
            : this(outputFiles, Array.Empty<ScriptError>())
        {
        }

        internal ScriptResult(ICollection<IOutputFileInfo> outputFiles, ICollection<ScriptError> errors)
        {
            OutputFiles = outputFiles;
            Errors = errors;
        }
    }
}