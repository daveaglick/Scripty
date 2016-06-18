namespace Scripty.Core.Output
{
    public interface IOutputFileInfo
    {
        string FilePath { get; }    
        BuildAction BuildAction { get; }
    }
}