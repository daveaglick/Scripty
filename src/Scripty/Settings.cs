namespace Scripty
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.CommandLine;
    using Core;

    internal class Settings
    {
        public string ProjectFilePath;
        public IReadOnlyList<string> ScriptFilePaths;
        public bool Attach;

        private bool _showFullExceptionDetails;
        private bool _noOutputOnCompileErr;
        private bool _neverOverwriteOutput;
        private bool _scriptControlsOutput;
        public bool NoOutputOnCompileErr => _noOutputOnCompileErr;
        public bool NeverOverwriteOutput => _neverOverwriteOutput;
        public bool ScriptControlsOutput => _scriptControlsOutput;
        public bool ShowFullExceptionDetails => _showFullExceptionDetails;

        public bool ParseArgs(string[] args, out bool hasErrors)
        {
            //this new System.CommandLine seems like a good idea, but I think MS has made some real garbage with it.
            var parsed = ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("e", ref _showFullExceptionDetails, "Display full exception details instead of just the message");
                syntax.DefineOption("attach", ref Attach, "Pause execution at the start of the program until a debugger is attached.");
                syntax.DefineOption("outnoc", ref _noOutputOnCompileErr, "Do not produce or overwrite output on compile errors. This is the default.");
                syntax.DefineOption("outnev", ref _neverOverwriteOutput, "Do not produce or overwrite output.");
                syntax.DefineOption("outscr", ref _scriptControlsOutput, "The script determines what output is retained.");
                syntax.DefineParameter(nameof(ProjectFilePath), ref ProjectFilePath, "The full path of the project file.");
                syntax.DefineParameterList(nameof(ScriptFilePaths), ref ScriptFilePaths,
                    "The path(s) of script files to evaluate (can be absolute or relative to the project).");
            });
            GetOutputBehavior();
            hasErrors = parsed.HasErrors;
            return !(parsed.IsHelpRequested() || hasErrors);
        }

        public OutputBehavior GetOutputBehavior()
        {
            var obscore = (NoOutputOnCompileErr == true ? 1 : 0)
                          + (NeverOverwriteOutput == true ? 1 : 0)
                          + (ScriptControlsOutput == true ? 1 : 0);

            if (obscore == 0)
            {
                return OutputBehavior.DontOverwriteIfEvaluationFails;
            }

            if (obscore > 1)
            {
                throw new ArgumentException("Only one output behavior can be specified");
            }

            if (NeverOverwriteOutput)
            {
                return OutputBehavior.NeverGenerateOutput;
            }

            if (ScriptControlsOutput)
            {
                return  OutputBehavior.ScriptControlsOutput;
            }

            return OutputBehavior.DontOverwriteIfEvaluationFails;
        }
    }
}