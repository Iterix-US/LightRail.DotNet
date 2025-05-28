using SeroGlint.DotNet.NamedPipes.Delegates;
using System;
using System.Threading.Tasks;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface INamedPipeServer : IDisposable
    {
        event PipeMessageReceivedHandler MessageReceived;
        Task StartAsync();
    }
}
