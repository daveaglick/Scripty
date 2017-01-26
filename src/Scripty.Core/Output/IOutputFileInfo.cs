using Microsoft.CodeAnalysis.Options;

namespace Scripty.Core.Output
{
    public interface IOutputFileInfo
    {
        string TargetFilePath { get; }
        string TempFilePath { get; }
        BuildAction BuildAction { get; }
        bool FormatterEnabled { get; }
        FormatterOptions FormatterOptions { get; }
        bool KeepOutput { get; set; }
        bool OutputWasGenerated { get; set; }
        bool IsClosed { get; }
    }
}