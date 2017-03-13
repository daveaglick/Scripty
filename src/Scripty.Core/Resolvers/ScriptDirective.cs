namespace Scripty.Core.Resolvers
{
    /// <summary>
    ///     A script directive
    /// </summary>
    public class ScriptDirective
    {
        #region "props and fields"

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public DirectiveType Type { get; set; }

        /// <summary>
        ///     Gets or sets the resolution target type.
        /// </summary>
        public ResolutionTargetType ResolutionTargetType { get; set; }

        /// <summary>
        ///     The value of line where the directive appeared
        /// </summary>
        public string RawValue { get; }

        /// <summary>
        ///     Gets or sets the original reference path.
        /// </summary>
        public string OriginalReferencePath { get; set; }

        /// <summary>
        ///     The full path of the resolved reference
        /// </summary>
        public string RewrittenReferencePath { get; private set; }

        /// <summary>
        ///     The full path of calling script.
        /// </summary>
        public string PathOfCallingScript { get; }

        /// <summary>
        ///     Gets or sets the calling script line number. Zero based
        /// </summary>
        public int CallingScriptLineNumber { get; set; }

        /// <summary>
        ///     Gets the resolution required.
        /// </summary>
        public bool RewriteOfReferenceWasNeeded => _rewriteOfReferenceWasNeeded;

        private bool _rewriteOfReferenceWasNeeded;

        #endregion //#region "props and fields"

        #region "ctors"

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScriptDirective"/> class.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        /// <param name="pathOfCallingScript">The path of calling script.</param>
        /// <param name="callingScriptLineNumber">The calling script line number.</param>
        /// <param name="originalReferencePath">The original reference path.</param>
        /// <param name="type">The type.</param>
        public ScriptDirective(string rawValue, string pathOfCallingScript, int callingScriptLineNumber, string originalReferencePath, DirectiveType type)
        {
            RawValue = rawValue;
            PathOfCallingScript = pathOfCallingScript;
            CallingScriptLineNumber = callingScriptLineNumber;
            OriginalReferencePath = originalReferencePath;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptDirective" /> class.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        public ScriptDirective(string rawValue)
        {
            RawValue = rawValue;
        }

        #endregion //#region "ctors"

        #region "members"

        /// <summary>
        ///     Sets the resolved path.
        /// </summary>
        /// <param name="resolvedPath">The resolved path.</param>
        public void SetRewrittenReferncePath(string resolvedPath)
        {
            _rewriteOfReferenceWasNeeded = true;
            RewrittenReferencePath = resolvedPath;
        }

        #endregion //#region "members"
    }
}