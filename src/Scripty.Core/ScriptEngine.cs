using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Scripting;
using Scripty.Core.Output;
using Scripty.Core.ProjectTree;

namespace Scripty.Core
{
    using System.Diagnostics;

    public class ScriptEngine
    {
        private readonly string _projectFilePath;

        public OutputBehavior OutputBehavior { get; set; }

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

            var scriptResult = new ScriptResult();
            Exception caughtException = null;


            using (ScriptContext context = GetContext(source.FilePath))
            {
                bool writeAllOutputFiles = true;

                try
                {
                    await CSharpScript.EvaluateAsync(source.Code, options, context);
                    scriptResult.OutputFiles = context.Output.OutputFileInfos;
                }
                catch (CompilationErrorException compilationError)
                {
                    caughtException = compilationError;
                    scriptResult.OutputFiles = context.Output.OutputFileInfos;
                    scriptResult.Errors = compilationError.Diagnostics
                        .Select(x => new ScriptError
                        {
                            Message = x.GetMessage(),
                            Line = x.Location.GetLineSpan().StartLinePosition.Line,
                            Column = x.Location.GetLineSpan().StartLinePosition.Character,
                            FilePath = x.Location.GetLineSpan().Path
                        })
                        .ToList();
                }
                catch (AggregateException aggregateException)
                {
                    caughtException = aggregateException;

                    scriptResult.OutputFiles = context.Output.OutputFileInfos;
                    scriptResult.Errors = aggregateException.InnerExceptions
                            .Select(x => new ScriptError
                            {
                                Message = x.ToString()
                            }).ToList();
                }
                catch (Exception ex)
                {
                    caughtException = ex;

                    scriptResult.OutputFiles = context.Output.OutputFileInfos;
                    scriptResult.Errors = new[] {
                        new ScriptError
                        {
                                Message = ex.ToString()
                            }
                        };
                }

                switch (OutputBehavior)
                {

                    case OutputBehavior.DontOverwriteIfEvaluationFails:
                        if (caughtException != null)
                        {
                            //future - if compilation error, do something, else do something else
                            writeAllOutputFiles = false;
                        }
                        break;

                    case OutputBehavior.ScriptControlsOutput:
                        writeAllOutputFiles = true; // this will be examined in the WriteAllOutputFiles
                        break;

                    case OutputBehavior.NeverGenerateOutput:
                        writeAllOutputFiles = false;
                        break;
                }

                if (writeAllOutputFiles)
                {
                    await WriteAllOutputFiles(context);
                }
                context.Output.CleanupAllTempData();
            }
            return scriptResult;
        }

        protected async Task WriteAllOutputFiles(ScriptContext context)
        {
            foreach (var outputFile in context.Output.GetOutputFilesForWriting())
            {

                if (File.Exists(outputFile.TargetFilePath))
                {
                    File.Delete(outputFile.TargetFilePath);
                }

                //if temp files not wanted, perhaps here is where a memory stream could be created
                File.Move(outputFile.TempFilePath, outputFile.TargetFilePath);
                outputFile.OutputWasGenerated = true;

                if (outputFile.FormatterEnabled)
                {
                    var document = ProjectRoot.Analysis.AddDocument(outputFile.TargetFilePath, File.ReadAllText(outputFile.TargetFilePath));

                    var resultDocument = await Formatter.FormatAsync(document,
                        outputFile.FormatterOptions.Apply(ProjectRoot.Workspace.Options)
                    );
                    var resultContent = await resultDocument.GetTextAsync();

                    File.WriteAllText(outputFile.TargetFilePath, resultContent.ToString());
                }
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
