using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;

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

        public string SolutionFilePath { get; set; }

        public string ScriptyExecutable { get; set; }

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

            if (string.IsNullOrEmpty(ScriptyExecutable))
            {
                ScriptyExecutable = Path.Combine(Path.GetDirectoryName(typeof(ScriptyTask).Assembly.Location), "Scripty.exe");
            }

            if (!File.Exists(ScriptyExecutable))
            {
                Log.LogError($"Scripty executable not found at '{ScriptyExecutable}'.");
                return false;
            }

            // Kick off the evaluation process, which must be done in a seperate process space
            // otherwise MSBuild complains when we construct the Roslyn workspace project since
            // it uses MSBuild to figure out what the project contains and MSBuild only supports
            // one build per process
            Log.LogMessage("Starting out-of-process script evaluation...");
            List<string> outputData = new List<string>();
            List<string> errorData = new List<string>();
            Process process = new Process();
            process.StartInfo.FileName = ScriptyExecutable;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.OutputDataReceived += (s, e) => outputData.Add(e.Data);
            process.ErrorDataReceived += (s, e) => errorData.Add(e.Data);
            process.Start();

            // Create and send the settings
            string settingsJson = GetSettingsJson();
            process.StandardInput.Write(settingsJson);
            process.StandardInput.Flush();
            process.StandardInput.Close();

            // Wait for the process to exit
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

            var messages = errorData.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split(new[] { '|' }, 2))
                .Where(x => x.Length == 2)
                .Select(x => new { MessageType = (MessageType)Enum.Parse(typeof(MessageType), x[0]), Message = x[1] })
                .ToArray();

            // Report any errors
            foreach (var message in messages)
            {
                switch (message.MessageType)
                {
                    case MessageType.Info:
                        Log.LogMessage(message.Message);
                        break;
                    case MessageType.Warning:
                        Log.LogWarning(message.Message);
                        break;
                    case MessageType.Error:
                        Log.LogError(message.Message);
                        break;
                }
            }

            if (messages.Where(m => m.MessageType == MessageType.Error).Any())
            {
                return false;
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

            return !Log.HasLoggedErrors;
        }

        private string GetSettingsJson()
        {
            Settings settings = new Settings
            {
                MessagesEnabled = true,
                ProjectFilePath = ProjectFilePath,
                Properties = GetMsBuildProperties(),
                ScriptFilePaths = ScriptFiles
                    .Select(x => x.GetMetadata("FullPath"))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct()
                    .ToList(),
                SolutionFilePath = SolutionFilePath?.Contains("*Undefined*") == true ? null : SolutionFilePath
            };
            return JsonConvert.SerializeObject(settings);
        }

        private IDictionary<string, string> GetMsBuildProperties()
        {
            // We need to use reflection to get
            // the build properties out of MSBuild.
            try
            {
                int version = BuildEngine.GetType().Assembly.GetName().Version.Major;

                // The name of the field that stores the IBuildComponentHost changed in MSBuild 14.
                object host = BuildEngine.GetType().InvokeMember(
                    (version >= 14) ? "_host" : "host",
                    BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    BuildEngine,
                    new object[] { }
                );

                object buildParameters = host.GetType().GetInterface("IBuildComponentHost").InvokeMember(
                    "BuildParameters",
                    BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    host,
                    new object[] { }
                );

                object globalProperties = buildParameters.GetType().InvokeMember(
                    "GlobalProperties",
                    BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    buildParameters,
                    new object[] { }
                );

                return (IDictionary<string, string>)globalProperties;
            }
            catch (Exception ex)
            {
                Log.LogWarning("Could not get global properties from MSBuild: " + ex.Message);
                return null;
            }
        }
    }
}
