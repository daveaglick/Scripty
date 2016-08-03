using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripty
{
    internal class Settings
    {
        public string ProjectFilePath = null;
        public IReadOnlyList<string> ScriptFilePaths = null;
        public bool Attach = false;

        public bool ParseArgs(string[] args, out bool hasErrors)
        {
            System.CommandLine.ArgumentSyntax parsed = System.CommandLine.ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("attach", ref Attach, "Pause execution at the start of the program until a debugger is attached.");
                syntax.DefineParameter(nameof(ProjectFilePath), ref ProjectFilePath, "The full path of the project file.");
                syntax.DefineParameterList(nameof(ScriptFilePaths), ref ScriptFilePaths, "The path(s) of script files to evaluate (can be absolute or relative to the project).");
            });

            hasErrors = parsed.HasErrors;
            return !(parsed.IsHelpRequested() || hasErrors);
        }
    }
}
