using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Scripty
{
    /// <summary>
    /// A runner for evaluating Scripty scripts in a project
    /// </summary>
    public class ScriptyRunner : Tool<ScriptySettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptyRunner"/> class
        /// </summary>
        /// <param name="fileSystem">The file system</param>
        /// <param name="environment">The environment</param>
        /// <param name="processRunner">The process runner</param>
        /// <param name="tools">The tools</param>
        /// <param name="projectFilePath">Path to the project file</param>
        public ScriptyRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator tools, FilePath projectFilePath)
            : base(fileSystem, environment, processRunner, tools)
        {
            if (projectFilePath == null) throw new ArgumentNullException(nameof(projectFilePath));
            ProjectFilePath = projectFilePath;
            Environment = environment;
        }

        private ICakeEnvironment Environment { get; set; }

        private FilePath ProjectFilePath { get; set; }

        private ScriptySettings Settings { get; set; } = new ScriptySettings();

        /// <summary>
        /// Evaluates the given Scripty scripts
        /// </summary>
        /// <param name="scriptFiles">The script files to evaluate</param>
        public void Evaluate(IEnumerable<FilePath> scriptFiles)
        {
            var filePaths = scriptFiles as IList<FilePath> ?? scriptFiles.ToList();
            if (!filePaths.Any())
            {
                throw new ArgumentException("No files provided to evaluate", nameof(scriptFiles));
            }
            var args = new ProcessArgumentBuilder();
            args.AppendQuoted(ProjectFilePath.MakeAbsolute(Environment).FullPath);
            foreach (var scriptFile in filePaths)
            {
                var path = ProjectFilePath.MakeAbsolute(Environment).GetRelativePath(scriptFile.MakeAbsolute(Environment)).FullPath;
                args.AppendQuoted(path);
            }
            Run(Settings, args);
        }

        /// <summary>
        /// Evaluates the given Scripty scripts
        /// </summary>
        /// <param name="scriptFiles">The script files to evaluate</param>
        public void Evaluate(params FilePath[] scriptFiles)
        {
            Evaluate(scriptFiles.AsEnumerable());
        }

        /// <summary>Gets the name of the tool.</summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Scripty";
        }

        /// <summary>Gets the possible names of the tool executable.</summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "scripty.exe";
            yield return "scripty";
        }
    }
}