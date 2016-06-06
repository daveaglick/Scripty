namespace Scripty.Core
{
    public interface IOutputFileInfo
    {
        string FilePath { get; }    
        BuildAction BuildAction { get; }
    }
}