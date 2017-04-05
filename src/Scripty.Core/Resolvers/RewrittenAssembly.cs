namespace Scripty.Core.Resolvers
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.CodeAnalysis.Emit;

    /// <summary>
    ///     A single code file that has had some content removed and then compiled 
    /// </summary>
    public class RewrittenAssembly
    {
        /// <summary>
        ///     Gets or sets the original file path.
        /// </summary>
        public string OriginalFilePath { get; set; }
        /// <summary>
        ///     Gets or sets the assembly file path.
        /// </summary>
        public string AssemblyFilePath { get; set; }
        /// <summary>
        ///     Gets or sets the assembly bytes.
        /// </summary>
        public byte[] AssemblyBytes { get; set; }
        /// <summary>
        ///     Gets or sets the PDB file path.
        /// </summary>
        public string PdbFilePath { get; set; }
        /// <summary>
        ///     Gets or sets the PDB bytes.
        /// </summary>
        public byte[] PdbBytes { get; set; }

        /// <summary>
        ///     Any namepaces found during rewrite.
        /// </summary>
        public List<string> FoundNamespaces { get;  }  = new List<string>();

        /// <summary>
        ///     Any assemblies found during rewrite.
        /// </summary>
        public List<Assembly> FoundAssemblies { get;  } = new List<Assembly>();

        /// <summary>
        ///     Gets the is compiled.
        /// </summary>
        public bool IsCompiled
        {
            get { return AssemblyBytes != null && AssemblyBytes.Length > 0; }
        }

        /// <summary>
        ///     Gets or sets the compilation result.
        /// </summary>
        public EmitResult CompilationResult { get; set; }
    }
}