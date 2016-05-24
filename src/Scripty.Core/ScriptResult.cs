using System;
using System.Collections.Generic;

namespace Scripty.Core
{
    public class ScriptResult
    {
        public ICollection<string> Errors { get; }

        internal ScriptResult(ICollection<string> errors)
        {
            Errors = errors;
        }

        public ScriptResult()
        {
            Errors = Array.Empty<string>();
        }
    }
}