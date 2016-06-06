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
        private readonly List<ITaskItem> _noneFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _compileFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _contentFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _embeddedResourceFiles = new List<ITaskItem>();

        [Required]
        public string ProjectFilePath { get; set; }

        public ITaskItem[] ScriptFiles { get; set; }

        [Output]
        public ITaskItem[] NoneFiles => _noneFiles.ToArray();

        [Output]
        public ITaskItem[] CompileFiles => _compileFiles.ToArray();

        [Output]
        public ITaskItem[] ContentFiles => _contentFiles.ToArray();

        [Output]
        public ITaskItem[] EmbeddedResourceFiles => _embeddedResourceFiles.ToArray();

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
            List<Tuple<BuildAction, string>> outputFiles = outputData
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split('|'))
                .Where(x => x.Length == 2 && !string.IsNullOrWhiteSpace(x[0]) && !string.IsNullOrWhiteSpace(x[1]))
                .Select(x =>
                {
                    BuildAction buildAction;
                    if (!Enum.TryParse(x[0], out buildAction))
                    {
                        buildAction = BuildAction.GenerateOnly;
                    }
                    return new Tuple<BuildAction, string>(buildAction, x[1]);
                })
                .Where(x => x.Item1 != BuildAction.GenerateOnly)
                .ToList();

            Log.LogMessage("Output file count: " + outputFiles.Count);

            _noneFiles.AddRange(outputFiles
                .Where(x => x.Item1 == BuildAction.None)
                .Select(x =>
                {
                    TaskItem taskItem = new TaskItem(x.Item2);
                    taskItem.SetMetadata("AutoGen", "true");
                    return taskItem;
                }));

            _compileFiles.AddRange(outputFiles
                .Where(x => x.Item1 == BuildAction.Compile)
                .Select(x =>
                {
                    TaskItem taskItem = new TaskItem(x.Item2);
                    taskItem.SetMetadata("AutoGen", "true");
                    return taskItem;
                }));

            _contentFiles.AddRange(outputFiles
                .Where(x => x.Item1 == BuildAction.Content)
                .Select(x =>
                {
                    TaskItem taskItem = new TaskItem(x.Item2);
                    taskItem.SetMetadata("AutoGen", "true");
                    return taskItem;
                }));

            _embeddedResourceFiles.AddRange(outputFiles
                .Where(x => x.Item1 == BuildAction.EmbeddedResource)
                .Select(x =>
                {
                    TaskItem taskItem = new TaskItem(x.Item2);
                    taskItem.SetMetadata("AutoGen", "true");
                    return taskItem;
                }));

            return true;
        }
    }
}
