using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Scripty.MsBuild
{
    public class ScriptyTask : Microsoft.Build.Utilities.Task
    {
        private readonly List<ITaskItem> _compileFiles = new List<ITaskItem>();

        [Required]
        public string ProjectFilePath { get; set; }

        public ITaskItem[] ScriptFiles { get; set; }

        [Output]
        public ITaskItem[] CompileFiles => _compileFiles.ToArray();

        public override bool Execute()
        {
            if (ScriptFiles == null || ScriptFiles.Length == 0)
            {
                return true;
            }
            if (string.IsNullOrEmpty(ProjectFilePath))
            {
                Log.LogError("A project file is required");
                return false;
            }
            if (!Path.IsPathRooted(ProjectFilePath))
            {
                Log.LogError("The project file path must be absolute");
                return false;
            }

            // Get the script files and construct the arguments to pass to the CLI
            string scriptFiles = string.Join(" ", ScriptFiles
                .Select(x => x.GetMetadata("FullPath"))
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .Select(x => "\"" + x + "\""));

            // Kick off the evaluation process, which must be done in a seperate process space
            // otherwise MSBuild complains when we construct the Roslyn workspace project since
            // it uses MSBuild to figure out what the project contains and MSBuild only supports
            // one build per process
            Log.LogMessage("Starting out-of-process script evaluation...");
            string arguments = $"\"{ProjectFilePath}\" {scriptFiles}";
            Log.LogMessage("Arguments: " + arguments);
            List<string> outputData = new List<string>();
            List<string> errorData = new List<string>();
            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(typeof(ScriptyTask).Assembly.Location), "Scripty.exe");
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += (s, e) => outputData.Add(e.Data);
            process.ErrorDataReceived += (s, e) => errorData.Add(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                Log.LogMessage("Finished script evaluation");
            }
            else
            {
                Log.LogError("Got non-zero exit code from script evaluation: " + process.ExitCode);
            }

            // Report any errors
            foreach (string error in errorData.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                Log.LogError(error);
            }

            // Add the compile files
            List<string> compileFiles = outputData
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
            Log.LogMessage("Compile file count: " + compileFiles.Count);
            _compileFiles.AddRange(compileFiles
                .Select(x =>
                {
                    TaskItem taskItem = new TaskItem(x);
                    taskItem.SetMetadata("AutoGen", "true");
                    return taskItem;
                }));

            return true;
        }
    }
}
