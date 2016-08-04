using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scripty.Core;
using Scripty.Core.Output;
using Scripty.Core.ProjectTree;

namespace Scripty
{
    public class Program
    {
        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEvent;
            Program program = new Program();
            return program.Run(args);
        }

        private static void UnhandledExceptionEvent(object sender, UnhandledExceptionEventArgs e)
        {
            // Exit with a error exit code
            Exception exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                Console.Error.WriteLine(exception.ToString());
            }
            Environment.Exit((int) ExitCode.UnhandledError);
        }

        private readonly Settings _settings = new Settings();

        private int Run(string[] args)
        {
            // Parse the command line
            try
            {
                bool hasParseArgsErrors;
                if (!_settings.ParseArgs(args, out hasParseArgsErrors))
                {
                    return hasParseArgsErrors ? (int)ExitCode.CommandLineError : (int)ExitCode.Normal;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return (int)ExitCode.CommandLineError;
            }

            // Attach
            if (_settings.Attach)
            {
                Console.WriteLine("Waiting for a debugger to attach (or press a key to continue)...");
                while (!Debugger.IsAttached && !Console.KeyAvailable)
                {
                    Thread.Sleep(100);
                }
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Console.WriteLine("Key pressed, continuing execution");
                }
                else
                {
                    Console.WriteLine("Debugger attached, continuing execution");
                }
            }

            // Get the script engine
            ScriptEngine engine = new ScriptEngine(_settings.ProjectFilePath);

            // Get script files if none were specified
            IReadOnlyList<string> finalScriptFilePaths;
            if (_settings.ScriptFilePaths != null && _settings.ScriptFilePaths.Count > 0)
            {
                finalScriptFilePaths = _settings.ScriptFilePaths;
            }
            else
            {
                // Look for any .csx files in the project
                Console.WriteLine("No script files were specified, scanning project for .csx files");
                List<string> scriptFilePaths = new List<string>();
                PopulateScriptFilePaths(engine.ProjectRoot, scriptFilePaths);
                finalScriptFilePaths = scriptFilePaths;
            }
            
            // Set up tasks for the specified script files
            ConcurrentBag<Task<ScriptResult>> tasks = new ConcurrentBag<Task<ScriptResult>>();
            Parallel.ForEach(finalScriptFilePaths
                .Select(x => Path.Combine(Path.GetDirectoryName(_settings.ProjectFilePath), x))
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct(),
                x =>
                {
                    if (File.Exists(x))
                    {
                        Console.WriteLine($"Adding task to evaluate {x}");
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
                    Console.Error.WriteLine(ex.ToString());
                }
            }

            // Iterate over the completed tasks
            foreach (Task<ScriptResult> task in tasks.Where(x => x.Status == TaskStatus.RanToCompletion))
            {
                // Check for any errors
                foreach (ScriptError error in task.Result.Errors)
                {
                    Console.Error.WriteLine($"{error.Message} [{error.Line},{error.Column}]");
                }

                // Output the set of generated files w/ build actions
                foreach (IOutputFileInfo outputFile in task.Result.OutputFiles)
                {
                    Console.WriteLine($"{outputFile.BuildAction}|{outputFile.FilePath}");
                }
            }

            return (int) ExitCode.Normal;
        }

        private void PopulateScriptFilePaths(ProjectNode node, List<string> scriptFilePaths)
        {
            foreach (KeyValuePair<string, ProjectNode> child in node.Children)
            {
                if (Path.GetExtension(child.Key) == ".csx")
                {
                    scriptFilePaths.Add(child.Key);
                }
                PopulateScriptFilePaths(child.Value, scriptFilePaths);
            }
        }
    }
}
