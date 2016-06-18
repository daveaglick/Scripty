namespace Scripty.Core.Output
{
    public enum BuildAction
    {
        /// <summary>
        /// Only generates the file but does not add it to the project.
        /// </summary>
        GenerateOnly,

        /// <summary>
        /// Adds the file to the project and does not set a build action.
        /// </summary>
        None,

        /// <summary>
        /// Adds the file to the project and sets the compile build action.
        /// </summary>
        Compile,

        /// <summary>
        /// Adds the file to the project and sets the content build action.
        /// </summary>
        Content,

        /// <summary>
        /// Adds the file to the project and sets the embedded resource build action.
        /// </summary>
        EmbeddedResource
    }
}
