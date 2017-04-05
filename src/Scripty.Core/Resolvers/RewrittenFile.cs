namespace Scripty.Core.Resolvers
{
    /// <summary>
    ///     A single code file that has had some content removed and saved to disk
    /// </summary>
    public class RewrittenFile
    {
        public string OriginalFilePath { get; set; }
        public string RewrittenFilePath { get; set; }
    }
}
