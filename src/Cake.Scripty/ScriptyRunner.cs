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
        private ICakeEnvironment _environment { get; set; }

        private ScriptySettings _settings { get; }

        private FilePath _projectFilePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptyRunner"/> class
        /// </summary>
        /// <param name="fileSystem">The file system</param>
        /// <param name="environment">The environment</param>
        /// <param name="processRunner">The process runner</param>
        /// <param name="tools">The tools</param>
        /// <param name="projectFilePath">Path to the project file</param>
        /// <param name="settings">Settings for running the tool</param>
        public ScriptyRunner(
            IFileSystem fileSystem, 
            ICakeEnvironment environment, 
            IProcessRunner processRunner,
            IToolLocator tools, 
            FilePath projectFilePath, 
            ScriptySettings settings
        )
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
            _projectFilePath = projectFilePath ?? throw new ArgumentNullException(nameof(projectFilePath));
            _settings = settings ?? new ScriptySettings();
        }

        /// <summary>
        /// Evaluates the given Scripty scripts
        /// </summary>
        /// <param name="scriptFiles">The script files to evaluate</param>
        public void Evaluate(params FilePath[] scriptFiles)
        {
            Evaluate(scriptFiles.AsEnumerable());
        }

        /// <summary>
        /// Evaluates the given Scripty scripts
        /// </summary>
        /// <param name="scriptFiles">The script files to evaluate</param>
        public void Evaluate(IEnumerable<FilePath> scriptFiles)
        {
            if (scriptFiles.Any() == false)
            {
                throw new ArgumentException("No files provided to evaluate", nameof(scriptFiles));
            }

            var scriptWorkingDirectory = ScriptWorkingDirectory();
            var absoluteProjectPath = _projectFilePath.MakeAbsolute(scriptWorkingDirectory);

            var args = new ProcessArgumentBuilder();
            args.AppendQuoted(absoluteProjectPath.FullPath);
            foreach (var scriptFile in scriptFiles)
            {
                var path = absoluteProjectPath.GetRelativePath(scriptFile.MakeAbsolute(scriptWorkingDirectory)).FullPath;
                args.AppendQuoted(path);
            }

            Run(_settings, args);
        }

        /// <summary>
        /// Gets the working directory for where the scripts files are located
        /// </summary>
        /// <returns>The working directory</returns>
        private DirectoryPath ScriptWorkingDirectory()
        {
            if (_settings.WorkingDirectory == null)
            {
                return _environment.WorkingDirectory;
            }
            else if (_settings.WorkingDirectory.IsRelative)
            {
                return _settings.WorkingDirectory.MakeAbsolute(_environment.WorkingDirectory);
            }
            else
            {
                return _settings.WorkingDirectory;
            }
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