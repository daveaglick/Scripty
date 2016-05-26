using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Scripting;

namespace Scripty.Core
{
    public class ScriptEngine
    {
        private readonly string _projectFilePath;
        private readonly object _projectLock = new object();
        private Project _project;

        public ScriptEngine(string projectFilePath)
        {
            _projectFilePath = projectFilePath;

            // Uncomment to pause execution while waiting for a debugger to attach
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}
        }

        public async Task<ScriptResult> Evaluate(ScriptSource source)
        {
            ScriptOptions options = ScriptOptions.Default
                .WithFilePath(source.FilePath)
                .WithReferences(
                    typeof(Project).Assembly  // Microsoft.CodeAnalysis.Workspaces
                    )
                .WithImports(
                    "System",
                    "Microsoft.CodeAnalysis");

            using (ScriptGlobals globals = new ScriptGlobals(source.FilePath, _projectFilePath, LoadProject))
            {
                try
                {
                    await CSharpScript.EvaluateAsync(source.Code, options, globals);
                }
                catch (CompilationErrorException compilationError)
                {
                    return new ScriptResult(globals.Output.OutputFiles,
                        compilationError.Diagnostics.Select(x => x.ToString()).ToList());
                }
                catch (AggregateException aggregateException)
                {
                    return new ScriptResult(globals.Output.OutputFiles,
                        aggregateException.InnerExceptions.Select(x => x.ToString()).ToList());
                }
                catch (Exception ex)
                {
                    return new ScriptResult(globals.Output.OutputFiles,
                        new [] { ex.ToString() });
                }
                return new ScriptResult(globals.Output.OutputFiles);
            }
        }

        private Project LoadProject()
        {
            if (_project == null && !string.IsNullOrEmpty(_projectFilePath))
            {
                lock (_projectLock)
                {
                    MSBuildWorkspace workspace = MSBuildWorkspace.Create();
                    _project = workspace.OpenProjectAsync(_projectFilePath).Result;
                }
            }
            return _project;
        }
    }
}
