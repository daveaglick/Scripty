namespace Scripty.Core
{
    public interface IOutputFileInfo
    {
        string FilePath { get; }    
        bool Compile { get; }
    }
}