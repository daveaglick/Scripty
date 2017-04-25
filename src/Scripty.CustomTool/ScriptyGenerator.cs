using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Scripty.Core;
using Scripty.Core.Output;

namespace Scripty
{
    /// <summary>
    /// This is the generator class. 
    /// When setting the 'Custom Tool' property of a C# or VB project item to "Scripty", 
    /// the GenerateCode function will get called and will return the contents of the generated file 
    /// to the project system
    /// </summary>
    [ComVisible(true)]
    [Guid("1B8589A2-58FF-4413-9EA3-A66A1605F1E4")]
    [CodeGeneratorRegistration(typeof(ScriptyGenerator), "C# Scripty Generator", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(ScriptyGenerator), "VB.NET Scripty Generator", "{164B10B9-B200-11D0-8C61-00A0C91E29D5}", GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(ScriptyGenerator))]
    public class ScriptyGenerator : BaseCodeGeneratorWithSite
    {
#pragma warning disable 0414, 169
        //The name of this generator (use for 'Custom Tool' property of project item)
        // ReSharper disable once InconsistentNaming
        internal static string name = "ScriptyGenerator";
#pragma warning restore 0414, 169

        protected override string GetDefaultExtension()
        {
            return ".log";
        }

        /// <summary>
        /// Function that builds the contents of the generated file based on the contents of the input file.
        /// </summary>
        /// <param name="inputFileContent">Content of the input file</param>
        /// <returns>Generated file as a byte array</returns>
        protected override byte[] GenerateCode(string inputFileContent)
        {
            // Some good examples: https://t4toolbox.svn.codeplex.com/svn/Source/DteProcessor.cs
            // And https://github.com/madskristensen/ExtensibilityTools/blob/master/src/VsixManifest/Generator/ResxFileGenerator.cs

            try
            {
                ProjectItem projectItem = GetProjectItem();
                string inputFilePath = projectItem.Properties.Item("FullPath").Value.ToString();
                Project project = projectItem.ContainingProject;
                Solution solution = projectItem.DTE.Solution;

                // Run the generator and get the results
                ScriptSource source = new ScriptSource(inputFilePath, inputFileContent);
                ScriptEngine engine = new ScriptEngine(project.FullName, solution.FullName, null);
                ScriptResult result = engine.Evaluate(source).Result;

                // Report errors
                if (result.Errors.Count > 0)
                {
                    foreach (ScriptError error in result.Errors)
                    {
                        GeneratorError(4, error.Message, (uint) error.Line, (uint) error.Column);
                    }
                    return null;
                }
                
                // Add generated files to the project
                foreach (IOutputFileInfo outputFile in result.OutputFiles.Where(x => x.BuildAction != BuildAction.GenerateOnly))
                {
                    ProjectItem outputItem = null;
                    //Testing to make sure the path is valid. Invalid paths will cause the AddFromFile to fail with a null pointer exception
                    if (outputFile.FilePath.IndexOfAny(Path.GetInvalidPathChars()) > -1 || outputFile.FilePath.IndexOf(@"\\") > -1)
                    {
                        throw new Exception($"The Path provided, {outputFile.FilePath}, contains invalid characters.");
                    }

                    // Allotting for the ability to change the Project Name on a per file basis, which is why we are using a nested
                    // loop here. If there was no project Name set at all, assume the script project.
                    if (!string.IsNullOrWhiteSpace(outputFile.ProjectName))
                    {
                        foreach (Project p in solution.Projects)
                        {
                            if (p.Name == outputFile.ProjectName)
                            {
                                outputItem = p.ProjectItems.Cast<ProjectItem>()
                                            .FirstOrDefault(
                                                x =>
                                                    x.Properties.Item("FullPath")?.Value?.ToString() ==
                                                    outputFile.FilePath)
                                        ?? p.ProjectItems?.AddFromFile(outputFile.FilePath);
                            }

                        }
                    }
                    else
                    {
                        outputItem = projectItem.ProjectItems.Cast<ProjectItem>()
                                         .FirstOrDefault(
                                             x =>
                                                 x.Properties.Item("FullPath")?.Value?.ToString() ==
                                                 outputFile.FilePath)
                                     ?? projectItem.ProjectItems.AddFromFile(outputFile.FilePath);
                    }

                    if (outputItem != null)
                    {
                        outputItem.Properties.Item("ItemType").Value = outputFile.BuildAction.ToString();
                    }
                }

                // Remove/delete files from the last generation but not in this one
                string logPath = Path.ChangeExtension(inputFilePath, ".log");
                if (File.Exists(logPath))
                {
                    string[] logLines = File.ReadAllLines(logPath);
                    foreach (string fileToRemove in logLines.Where(x => result.OutputFiles.All(y => y.FilePath != x)))
                    {
                        solution.FindProjectItem(fileToRemove)?.Delete();
                    }
                }

                // Create the log file
                return Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, result.OutputFiles.Select(x => x.FilePath)));
            }
            catch (Exception ex)
            {
                GeneratorError(4, ex.ToString(), 0, 0);
                return null;
            }
        }
    }
}
