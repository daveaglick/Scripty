using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Execution;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Text;
using Scripty.Core.Output;
using Scripty.Core.ProjectTree;

namespace Scripty.Core
{
    public class ScriptEngine
    {
        private readonly string _projectFilePath;
        private readonly ProjectRoot _projectRoot;

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
            _projectRoot = new ProjectRoot(projectFilePath);

            // Uncomment to pause execution while waiting for a debugger to attach
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}
        }

        public ScriptContext GetContext(string scriptFilePath)
        {
            if (scriptFilePath == null)
            {
                throw new ArgumentNullException(nameof(scriptFilePath));
            }

            return new ScriptContext(scriptFilePath, _projectFilePath, _projectRoot);
        }

        public async Task<ScriptResult> Evaluate(ScriptSource source)
        {
            ScriptOptions options = ScriptOptions.Default
                .WithFilePath(source.FilePath)
                .WithReferences(
                    typeof(Microsoft.CodeAnalysis.Project).Assembly,  // Microsoft.CodeAnalysis.Workspaces
                    typeof(Microsoft.Build.Evaluation.Project).Assembly,  // Microsoft.Build
                    typeof(ScriptEngine).Assembly  // Scripty.Core
                    )
                .WithImports(
                    "System",
                    "Scripty.Core",
                    "Scripty.Core.Output",
                    "Scripty.Core.ProjectTree");

            using (ScriptContext context = GetContext(source.FilePath))
            {
                try
                {
                    await CSharpScript.EvaluateAsync(source.Code, options, context);
                    foreach (var outputFile in context.Output.OutputFiles)
                    {
                        (outputFile as OutputFile).Close();

                        if (outputFile.FormatterEnabled)
                        {
                            var document = _projectRoot.Analysis.AddDocument(outputFile.FilePath, File.ReadAllText(outputFile.FilePath));

                            var resultDocument = await Formatter.FormatAsync(
                                document,
                                outputFile.FormatterOptions
                            );
                            var resultContent = await resultDocument.GetTextAsync();

                            File.WriteAllText(outputFile.FilePath, resultContent.ToString());
                        }
                    }
                }
                catch (CompilationErrorException compilationError)
                {
                    return new ScriptResult(context.Output.OutputFiles,
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
                    return new ScriptResult(context.Output.OutputFiles,
                        aggregateException.InnerExceptions
                            .Select(x => new ScriptError
                            {
                                Message = x.ToString()
                            }).ToList());
                }
                catch (Exception ex)
                {
                    return new ScriptResult(context.Output.OutputFiles,
                        new[]
                        {
                            new ScriptError
                            {
                                Message = ex.ToString()
                            }
                        });
                }
                return new ScriptResult(context.Output.OutputFiles);
            }
        }
    }
}
