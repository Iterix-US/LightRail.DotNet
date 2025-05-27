using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SeroGlint.DotNet.NamedPipes.Servers
{
    public class PerpetualPipeServer : INamedPipeServer
    {
        private readonly INamedPipeServer _core;
        private bool _running;

        public event PipeMessageReceivedHandler MessageReceived
        {
            add => _core.MessageReceived += value;
            remove => _core.MessageReceived -= value;
        }

        public PerpetualPipeServer(INamedPipeServer core)
        {
            _core = core;
        }

        public async Task StartAsync(string pipeName, CancellationToken cancellationToken)
        {
            _running = true;
            while (_running && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _core.StartAsync(pipeName, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Pipe error: {ex.Message}");

                }
            }
        }

        public void Dispose()
        {
            _running = false;
            _core.Dispose();
        }
    }

}
