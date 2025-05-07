using System.Diagnostics.CodeAnalysis;
using SeroGlint.DotNet.Tests.Interfaces;

namespace SeroGlint.DotNet.Tests.Utilities
{
    [ExcludeFromCodeCoverage]
    public class LogFileCleaner : ILogFileCleaner
    {
        public bool TryDelete(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch (IOException)
            {
                Thread.Sleep(50);
                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch { return false; }
            }
        }
    }
}
