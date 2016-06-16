using System;
using System.Diagnostics;
using System.IO;
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
            if (string.IsNullOrEmpty(projectFilePath))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(projectFilePath));
            }
            if (!Path.IsPathRooted(projectFilePath))
            {
                throw new ArgumentException("Project path must be absolute", nameof(projectFilePath));
            }

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
                    typeof(Project).Assembly,  // Microsoft.CodeAnalysis.Workspaces
                    typeof(ScriptEngine).Assembly  // Scripty.Core
                    )
                .WithImports(
                    "System",
                    "Microsoft.CodeAnalysis",
                    "Scripty.Core");

            using (ScriptGlobals globals = new ScriptGlobals(source.FilePath, _projectFilePath, LoadProject))
            {
                try
                {
                    await CSharpScript.EvaluateAsync(source.Code, options, globals);
                }
                catch (CompilationErrorException compilationError)
                {
                    return new ScriptResult(globals.Output.OutputFiles,
                        compilationError.Diagnostics
                            .Select(x => new ScriptError
                            {
                                Message = x.GetMessage(),
                                Line = x.Location.GetLineSpan().StartLinePosition.Line,
                                Column = x.Location.GetLineSpan().StartLinePosition.Character
                            })
                            .ToList());
                }
                catch (AggregateException aggregateException)
                {
                    return new ScriptResult(globals.Output.OutputFiles,
                        aggregateException.InnerExceptions
                            .Select(x => new ScriptError
                            {
                                Message = x.ToString()
                            }).ToList());
                }
                catch (Exception ex)
                {
                    return new ScriptResult(globals.Output.OutputFiles,
                        new []
                        {
                            new ScriptError
                            {
                                Message = ex.ToString()
                            }
                        });
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
