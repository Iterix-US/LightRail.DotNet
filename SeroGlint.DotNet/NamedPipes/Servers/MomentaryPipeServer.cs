using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.NamedPipes.Delegates;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.NamedPipes.Packaging;

namespace SeroGlint.DotNet.NamedPipes.Servers
{
    /// <summary>
    /// A named pipe server that only processes the first message it receives and then disposes itself.
    /// </summary>
    [ExcludeFromCodeCoverage] // Excluded due to testing of the Core class
    public class MomentaryPipeServer : INamedPipeServerDecorator
    {
        private readonly ILogger _logger;
        public INamedPipeServerCore Core { get; }
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        private bool _hasReceivedMessage;

        /// <summary>
        /// Event that is triggered when a message is received on the named pipe server.
        /// </summary>
        public event PipeMessageReceivedHandler MessageReceived
        {
            add => Core.MessageReceived += value;
            remove => Core.MessageReceived -= value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MomentaryPipeServer"/> class.
        /// </summary>
        /// <param name="core"></param>
        /// <param name="logger"></param>
        public MomentaryPipeServer(INamedPipeServerCore core, ILogger logger)
        {
            _logger = logger;
            Core = core;
            Core.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, PipeMessageReceivedEventArgs e)
        {
            if (_hasReceivedMessage) return;

            _hasReceivedMessage = true;
            _logger.LogInformation("MomentaryPipeServer received first message. Cancelling server...");
            CancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Starts the named pipe server and processes only the first message it receives.
        /// </summary>
        /// <returns></returns>
        public Task StartAsync() => Core.StartAsync();

        /// <summary>
        /// Disposes the named pipe server, cancelling any ongoing operations and releasing resources.
        /// </summary>
        public void Dispose()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            Core.Dispose();
        }
    }
}
