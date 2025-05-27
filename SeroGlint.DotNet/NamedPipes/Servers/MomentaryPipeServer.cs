using System.Threading;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using System.Threading.Tasks;
using NLog;

namespace SeroGlint.DotNet.NamedPipes.Servers
{
    /// <summary>
    /// A named pipe server that only processes the first message it receives and then disposes itself.
    /// </summary>
    public class MomentaryPipeServer : INamedPipeServer
    {
        private readonly ILogger _logger;
        private readonly INamedPipeServer _inner;
        private bool _hasReceivedMessage;

        public event PipeMessageReceivedHandler MessageReceived
        {
            add => _inner.MessageReceived += value;
            remove => _inner.MessageReceived -= value;
        }

        public MomentaryPipeServer(INamedPipeServer core, ILogger logger)
        {
            _logger = logger;
            _inner = core;
            _inner.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, PipeMessageReceivedEventArgs e)
        {
            if (_hasReceivedMessage)
            {
                _logger.Warn("MomentaryPipeServer has already received a message. Ignoring subsequent messages.");
                return;
            }

            _logger.Info("MomentaryPipeServer received a message");
            _hasReceivedMessage = true;
            _inner.Dispose();
        }

        public Task StartAsync(string pipeName, CancellationToken cancellationToken) => _inner.StartAsync(pipeName, cancellationToken);

        public void Dispose() => _inner.Dispose();
    }
}
