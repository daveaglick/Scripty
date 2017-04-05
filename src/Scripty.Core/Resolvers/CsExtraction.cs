namespace Scripty.Core.Resolvers
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;

    /// <summary>
    ///     An extraction from a class file
    /// </summary>
    public class CsExtraction
    {
        /// <summary>
        ///     The namespace members to be compiled
        /// </summary>
        public ImmutableList<SyntaxTree> CompilationTargets { get; }

        /// <summary>
        ///     Any using directives found while extracting as well as expansions
        /// of namespaces that were removed as part of the extraction.
        /// </summary>
        public ImmutableList<string> Namespaces { get; }

        /// <summary>
        ///     Errors encountered
        /// </summary>
        public ImmutableList<string> Errors { get; }

        /// <summary>
        ///     Gets the extract file path.
        /// </summary>
        public string ExtractFilePath { get;  }

        /// <summary>
        ///     Gets the metadata references.
        /// </summary>
        public ImmutableList<MetadataReference> MetadataReferenceAssemblies { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsExtraction" /> class.
        /// </summary>
        /// <param name="metadataReferences"></param>
        /// <param name="compilationTargets">The compilation targets.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <param name="filePath">The file path.</param>
        public CsExtraction(IEnumerable<MetadataReference> metadataReferences, IEnumerable<SyntaxTree> compilationTargets, IEnumerable<string> namespaces, string filePath)
        {
            MetadataReferenceAssemblies = metadataReferences.ToImmutableList();
            CompilationTargets = compilationTargets.ToImmutableList();
            Namespaces = namespaces.ToImmutableList();
            ExtractFilePath = filePath;
            Errors = ImmutableList.Create<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsExtraction" /> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <param name="extractFilePath">The extract file path.</param>
        public CsExtraction(IEnumerable<string> errors, string extractFilePath)
        {
            Errors = errors.ToImmutableList();
            ExtractFilePath = extractFilePath;
        }
    }
}