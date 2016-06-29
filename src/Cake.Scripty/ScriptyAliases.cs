using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Scripty
{
    /// <summary>
    /// <para>Alias to evaluate C# scripts using Scripty</para>
    /// <code>#tool "nuget:?package=Scripty"</code>
    /// </summary>
    [CakeAliasCategory("Scripty")]
    public static class ScriptyAliases
    {
        /// <summary>
        /// Gets a <see cref="ScriptyRunner"/> to evaluate scripts for the given project
        /// </summary>
        /// <param name="context">The Cake context</param>
        /// <param name="projectFilePath">Path to the project file to use</param>
        /// <returns>A <see cref="ScriptyRunner"/> to evaluate scripts</returns>
        [CakeMethodAlias]
        public static ScriptyRunner Scripty(this ICakeContext context, FilePath projectFilePath)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context == null) throw new ArgumentNullException(nameof(projectFilePath));
            return new ScriptyRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, projectFilePath);
        }
    }
}