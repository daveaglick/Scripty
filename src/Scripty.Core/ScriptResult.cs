using System;
using System.Collections.Generic;
using Scripty.Core.Output;

namespace Scripty.Core
{
    public class ScriptResult
    {
        public ICollection<IOutputFileInfo> OutputFiles { get; }
        public ICollection<ScriptMessage> Messages { get; }

        internal ScriptResult(ICollection<IOutputFileInfo> outputFiles)
            : this(outputFiles, Array.Empty<ScriptMessage>())
        {
        }

        internal ScriptResult(ICollection<IOutputFileInfo> outputFiles, ICollection<ScriptMessage> errors)
        {
            OutputFiles = outputFiles;
            Messages = errors;
        }
    }
}