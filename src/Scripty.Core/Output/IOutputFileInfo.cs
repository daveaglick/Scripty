using Microsoft.CodeAnalysis.Options;

namespace Scripty.Core.Output
{
    public interface IOutputFileInfo
    {
        string FilePath { get; }
        BuildAction BuildAction { get; }
        bool FormatterEnabled { get; }
        FormatterOptions FormatterOptions { get; }
    }
}