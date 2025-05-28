using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.NamedPipes.Delegates;

namespace SeroGlint.DotNet.NamedPipes.Servers
{
    public class PerpetualPipeServer : INamedPipeServer
    {
        private readonly INamedPipeServerCore _core;
        private bool _running;

        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public event PipeMessageReceivedHandler MessageReceived
        {
            add => _core.MessageReceived += value;
            remove => _core.MessageReceived -= value;
        }

        public PerpetualPipeServer(INamedPipeServerCore core)
        {
            _core = core;
        }

        public async Task StartAsync()
        {
            _running = true;
            while (_running && !CancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    await _core.StartAsync();
                }
                catch (Exception ex)
                {
                    await RestartOnError(ex);
                }
            }
        }

        private async Task RestartOnError(Exception exception)
        {
            try
            {
                _core.Configuration.Logger.LogError(
                    new EventId((int)Error.OperationFailed),
                    exception,
                    "Could not start named pipe server.");

                await Task.Delay(TimeSpan.FromSeconds(2), CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                _core.Configuration.Logger.LogError(
                    new EventId((int)Error.OperationFailed),
                    ex,
                    "Error during restart delay for named pipe server.");
            }
        }

        public void Dispose()
        {
            _running = false;
            _core.Dispose();
        }
    }
}
