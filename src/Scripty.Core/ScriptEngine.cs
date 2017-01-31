using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Scripting;
using Scripty.Core.Output;
using Scripty.Core.ProjectTree;

namespace Scripty.Core
{
    public class ScriptEngine
    {
        private readonly string _projectFilePath;

        public ScriptEngine(string projectFilePath)
            : this(projectFilePath, null, null)
        {
        }

        public ScriptEngine(string projectFilePath, string solutionFilePath, IReadOnlyDictionary<string, string> properties)
        {
            if (string.IsNullOrEmpty(projectFilePath))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(projectFilePath));
            }
            if (!Path.IsPathRooted(projectFilePath))
            {
                throw new ArgumentException("Project path must be absolute", nameof(projectFilePath));
            }

            // The solution path is optional. If it's provided, the solution will be loaded and 
            // the project found in the solution. If not, then the project is loaded directly.
            if (solutionFilePath != null)
            {
                if (!Path.IsPathRooted(solutionFilePath))
                {
                    throw new ArgumentException("Solution path must be absolute", nameof(solutionFilePath));
                }
            }

            _projectFilePath = projectFilePath;
            ProjectRoot = new ProjectRoot(projectFilePath, solutionFilePath, properties);
        }

        public ProjectRoot ProjectRoot { get; }

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
                            var document = ProjectRoot.Analysis.AddDocument(outputFile.FilePath, File.ReadAllText(outputFile.FilePath));
                            
                            var resultDocument = await Formatter.FormatAsync(
                                document,
                                outputFile.FormatterOptions.Apply(ProjectRoot.Workspace.Options)
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

        private ScriptContext GetContext(string scriptFilePath)
        {
            if (scriptFilePath == null)
            {
                throw new ArgumentNullException(nameof(scriptFilePath));
            }

            return new ScriptContext(scriptFilePath, _projectFilePath, ProjectRoot);
        }
    }
}
