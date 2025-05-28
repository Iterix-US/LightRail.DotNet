using System.Threading;
using System.Threading.Tasks;
using NLog;
using SeroGlint.DotNet.NamedPipes.Delegates;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.NamedPipes.Packaging;

namespace SeroGlint.DotNet.NamedPipes.Servers
{
    /// <summary>
    /// A named pipe server that only processes the first message it receives and then disposes itself.
    /// </summary>
    public class MomentaryPipeServer : INamedPipeServer
    {
        private readonly ILogger _logger;
        private readonly INamedPipeServerCore _core;
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        private bool _hasReceivedMessage;

        public event PipeMessageReceivedHandler MessageReceived
        {
            add => _core.MessageReceived += value;
            remove => _core.MessageReceived -= value;
        }

        public MomentaryPipeServer(INamedPipeServerCore core, ILogger logger)
        {
            _logger = logger;
            _core = core;
            _core.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, PipeMessageReceivedEventArgs e)
        {
            if (_hasReceivedMessage) return;

            _hasReceivedMessage = true;
            _logger.Info("MomentaryPipeServer received first message. Cancelling server...");
            CancellationTokenSource.Cancel();
        }

        public Task StartAsync() => _core.StartAsync();

        public void Dispose()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            _core.Dispose();
        }
    }
}
