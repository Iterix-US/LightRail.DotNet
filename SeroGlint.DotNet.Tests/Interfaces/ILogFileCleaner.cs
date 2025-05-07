namespace SeroGlint.DotNet.Tests.Interfaces
{
    internal interface ILogFileCleaner
    {
        bool TryDelete(string filePath);
    }
}
