using SeroGlint.DotNet.NamedPipes.Packaging;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    /// <summary>
    /// Interface for the core functionality of a named pipe server.
    /// </summary>
    public interface INamedPipeServerCore : INamedPipeServer
    {
        /// <summary>
        /// Configuration settings for the named pipe server.
        /// </summary>
        PipeServerConfiguration Configuration { get; }
    }
}
