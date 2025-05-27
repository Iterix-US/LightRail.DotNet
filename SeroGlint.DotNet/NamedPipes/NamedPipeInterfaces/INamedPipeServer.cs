using System;
using System.Threading;
using System.Threading.Tasks;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface INamedPipeServer : IDisposable
    {
        event PipeMessageReceivedHandler MessageReceived;
        Task StartAsync(string pipeName, CancellationToken cancellationToken);
    }
}
