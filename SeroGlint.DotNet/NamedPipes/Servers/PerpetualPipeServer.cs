using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.NamedPipes.Delegates;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.NamedPipes.Servers
{
    /// <summary>
    /// A named pipe server that runs indefinitely, processing messages until explicitly stopped.
    /// </summary>
    [ExcludeFromCodeCoverage] // Excluded due to testing of the Core class
    public class PerpetualPipeServer : INamedPipeServerDecorator
    {
        public INamedPipeServerCore Core { get; }
        private bool _running;

        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// Event that is triggered when a message is received on the named pipe server.
        /// </summary>
        public event PipeMessageReceivedHandler MessageReceived
        {
            add => Core.MessageReceived += value;
            remove => Core.MessageReceived -= value;
        }

        public PerpetualPipeServer(INamedPipeServerCore core)
        {
            Core = core;
        }

        /// <summary>
        /// Starts the named pipe server and keeps it running until stopped or an error occurs.
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            _running = true;
            while (_running && !CancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    await Core.StartAsync();
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
                Core.Configuration.Logger.LogError(
                    new EventId((int)Error.OperationFailed),
                    exception,
                    "Could not start named pipe server.");

                await Task.Delay(TimeSpan.FromSeconds(2), CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Core.Configuration.Logger.LogError(
                    new EventId((int)Error.OperationFailed),
                    ex,
                    "Error during restart delay for named pipe server.");
            }
        }

        /// <summary>
        /// Disposes the named pipe server, stopping it from processing any further messages.
        /// </summary>
        public void Dispose()
        {
            _running = false;
            Core.Dispose();
        }
    }
}
