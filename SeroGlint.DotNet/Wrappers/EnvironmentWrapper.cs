using System;
using SeroGlint.DotNet.Abstractions;

namespace SeroGlint.DotNet.Wrappers
{
    public class EnvironmentWrapper : IEnvironmentWrapper
    {
        public void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        public string GetCommandLine()
        {
            return Environment.CommandLine;
        }

        public string GetBaseDirectory()
        {
            return AppContext.BaseDirectory;
        }
    }
}
