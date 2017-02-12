using Microsoft.CodeAnalysis.Options;

namespace Scripty.Core.Output
{
    public interface IOutputFileInfo
    {
        string FilePath { get; }
        // Added the ability to indicate which project the file should be added to.
        string ProjectName { get; set; }
        BuildAction BuildAction { get; }
        bool FormatterEnabled { get; }
        FormatterOptions FormatterOptions { get; }
    }
}