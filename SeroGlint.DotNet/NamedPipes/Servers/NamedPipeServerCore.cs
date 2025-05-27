using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace SeroGlint.DotNet.NamedPipes.Servers
{
    public class NamedPipeServerCore : INamedPipeServer
    {
        private readonly IPipeMessageFormatter _formatter;
        private readonly ILogger _logger;

        public event PipeMessageReceivedHandler MessageReceived;

        public NamedPipeServerCore(PipeConfiguration config, ILogger logger)
        {
            _logger = logger;
            _formatter = config.Formatter ?? new EncryptedJsonMessageFormatter(config.EncryptionKey, logger);
        }

        public async Task StartAsync(string pipeName, CancellationToken cancellationToken)
        {
            using (var server = new NamedPipeServerStream(
                       pipeName,
                       PipeDirection.InOut,
                       1,
                       PipeTransmissionMode.Message,
                       PipeOptions.Asynchronous))
            {
                await HandlePipeStream(pipeName, server, cancellationToken);
            }
        }

        private async Task HandlePipeStream(string pipeName, NamedPipeServerStream server, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Waiting for connection on pipe '{PipeName}'...", pipeName);
            await server.WaitForConnectionAsync(cancellationToken);
            _logger.LogInformation("Client connected on '{PipeName}'", pipeName);

            var buffer = new byte[4096];
            while (server.IsConnected && !cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Waiting for messages on pipe '{PipeName}'...", pipeName);
                var bytesRead = await server.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                if (bytesRead == 0)
                {
                    continue;
                }

                _logger.LogInformation("Received message on pipe '{PipeName}'", pipeName);
                var messageBytes = new byte[bytesRead];
                Array.Copy(buffer, 0, messageBytes, 0, bytesRead);

                var rawJson = Encoding.UTF8.GetString(messageBytes);
                var deserialized = _formatter.Deserialize<object>(messageBytes);
                _logger.LogInformation("Deserialized message on pipe '{PipeName}'", pipeName);

                MessageReceived?.Invoke(this, new PipeMessageReceivedEventArgs(rawJson, deserialized));
            }
        }

        public void Dispose() => _logger.LogInformation("NamedPipeServerCore disposed.");
    }
}
