using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Scripty.Core;
using Task = System.Threading.Tasks.Task;

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

            // Setup all the script sources and evaluation tasks
            ScriptEngine engine = new ScriptEngine(ProjectFilePath);
            ConcurrentBag<Task<ScriptResult>> tasks = new ConcurrentBag<Task<ScriptResult>>();
            Parallel.ForEach(ScriptFiles
                .Select(x => x.GetMetadata("FullPath"))
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct(), 
                x =>
                {
                    if (File.Exists(x))
                    {
                        tasks.Add(engine.Evaluate(new ScriptSource(x, File.ReadAllText(x))));
                    }
                });

            // Evaluate all the scripts
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException aggregateException)
            {
                foreach (Exception ex in aggregateException.InnerExceptions)
                {
                    Log.LogErrorFromException(ex);
                }
            }

            // Iterate over the completed tasks
            foreach (Task<ScriptResult> task in tasks.Where(x => x.Status == TaskStatus.RanToCompletion))
            {
                // Check for any errors
                foreach (string error in task.Result.Errors)
                {
                    Log.LogError(error);
                }

                // Add the compile files
                _compileFiles.AddRange(task.Result.OutputFiles
                    .Where(x => x.Compile)
                    .Select(x =>
                    {
                        TaskItem taskItem = new TaskItem(x.FilePath);
                        taskItem.SetMetadata("AutoGen", "true");
                        return taskItem;
                    }));
            }

            return true;
        }
    }
}
