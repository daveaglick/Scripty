using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripty
{
    public enum ExitCode
    {
        Normal = 0,
        UnhandledError = 1,
        CommandLineError = 2,
        EvaluationError = 3
    }
}
