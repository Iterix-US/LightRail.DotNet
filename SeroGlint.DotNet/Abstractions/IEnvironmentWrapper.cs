namespace SeroGlint.DotNet.Abstractions
{
    public interface IEnvironmentWrapper
    {
        void Exit(int exitCode);
        string GetCommandLine();
        string GetBaseDirectory();
    }
}