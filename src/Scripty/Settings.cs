using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scripty
{
    public class Settings
    {
        public string ProjectFilePath = null;
        public string SolutionFilePath = null;
        public IReadOnlyList<string> ScriptFilePaths = null;
        public IReadOnlyDictionary<string, string> Properties = null;
        public bool Attach = false;

        private IReadOnlyList<KeyValuePair<string, string>> _properties = null;

        public bool ParseArgs(string[] args, out bool hasErrors)
        {
            System.CommandLine.ArgumentSyntax parsed = System.CommandLine.ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("attach", ref Attach, "Pause execution at the start of the program until a debugger is attached.");
                syntax.DefineOption("solution", ref SolutionFilePath, "The full path of the solution file that contains the project.");
                syntax.DefineOptionList("p", ref _properties, ParseProperty, "The build properties.");
                syntax.DefineParameter(nameof(ProjectFilePath), ref ProjectFilePath, "The full path of the project file.");
                syntax.DefineParameterList(nameof(ScriptFilePaths), ref ScriptFilePaths, "The path(s) of script files to evaluate (can be absolute or relative to the project).");
            });

            if (_properties != null)
            {
                Dictionary<string, string> props = new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> pair in _properties)
                {
                    props[pair.Key] = pair.Value;
                }

                Properties = props;
            }

            hasErrors = parsed.HasErrors;
            return !(parsed.IsHelpRequested() || hasErrors);
        }

        private KeyValuePair<string, string> ParseProperty(string argument)
        {
            int index = argument.IndexOf('=');

            if (index < 0)
            {
                throw new InvalidOperationException("Malformed property argument.");
            }

            return new KeyValuePair<string, string>(
                argument.Substring(0, index),
                argument.Substring(index + 1)
            );
        }

        public void ReadStdin()
        {
            string stdin = StandardInputReader.Read();
            JsonConvert.PopulateObject(stdin, this);
        }
    }
}
