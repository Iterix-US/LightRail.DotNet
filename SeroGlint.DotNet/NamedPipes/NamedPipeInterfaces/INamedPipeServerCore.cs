using SeroGlint.DotNet.NamedPipes.Packaging;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface INamedPipeServerCore : INamedPipeServer
    {
        PipeServerConfiguration Configuration { get; }
    }
}
