using System;
using System.Threading.Tasks;
using SeroGlint.DotNet.NamedPipes.Delegates;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface INamedPipeServer : IDisposable
    {
        event PipeMessageReceivedHandler MessageReceived;
        Task StartAsync();
    }
}
