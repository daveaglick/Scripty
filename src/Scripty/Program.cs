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
            Program program = new Program();
            AppDomain.CurrentDomain.UnhandledException += program.UnhandledExceptionEvent;
            return program.Run(args);
        }

        private void WriteMessage(MessageType type, string message)
        {
            if (_settings.MessagesEnabled)
            {
                Console.Error.WriteLine(string.Format("{0}|{1}", type, message));
            }
            else if (type == MessageType.Error)
            {
                Console.Error.WriteLine(message);
            }
        }

        private void UnhandledExceptionEvent(object sender, UnhandledExceptionEventArgs e)
        {
            // Exit with a error exit code
            Exception exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                WriteMessage(MessageType.Error, exception.ToString());
            }
            Environment.Exit((int) ExitCode.UnhandledError);
        }

        private readonly Settings _settings = new Settings();

        private int Run(string[] args)
        {
            // Parse the command line if there are args
            if (args.Length > 0)
            {
                try
                {
                    bool hasParseArgsErrors;
                    if (!_settings.ParseArgs(args, out hasParseArgsErrors))
                    {
                        return hasParseArgsErrors ? (int) ExitCode.CommandLineError : (int) ExitCode.Normal;
                    }
                }
                catch (Exception ex)
                {
                    WriteMessage(MessageType.Error, ex.ToString());
                    return (int) ExitCode.CommandLineError;
                }
            }
            else
            {
                // Otherwise the settings should come in over stdin
                _settings.ReadStdin();
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
            string solutionFilePath = null;
            string projectFilePath = Path.Combine(Environment.CurrentDirectory, _settings.ProjectFilePath);

            if (_settings.SolutionFilePath != null)
            {
                solutionFilePath = Path.Combine(Environment.CurrentDirectory, _settings.SolutionFilePath);
            }

            ScriptEngine engine = new ScriptEngine(projectFilePath, solutionFilePath, _settings.Properties);

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
                .Select(x => Path.Combine(Path.GetDirectoryName(projectFilePath), x))
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
                    WriteMessage(MessageType.Error, ex.ToString());
                }
            }

            // Iterate over the completed tasks
            foreach (Task<ScriptResult> task in tasks.Where(x => x.Status == TaskStatus.RanToCompletion))
            {
                // Check for any mesages.
                foreach (ScriptMessage message in task.Result.Messages)
                {
                    if (message.Line == 0 || message.Column == 0)
                    {
                        WriteMessage(message.MessageType, message.Message);
                    }
                    else
                    {
                        WriteMessage(message.MessageType, $"{message.Message} [{message.Line},{message.Column}]");
                    }
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
