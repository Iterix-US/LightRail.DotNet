using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.Delegates;
using SeroGlint.DotNet.NamedPipes.EventArguments;
using SeroGlint.DotNet.NamedPipes.Interfaces;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.NamedPipes.Wrappers;

namespace SeroGlint.DotNet.NamedPipes
{
    /// <summary>
    /// A named pipe server that handles incoming messages and processes them asynchronously.
    /// </summary>
    public class NamedPipeServer : INamedPipeServer
    {
        public PipeServerConfiguration Configuration { get; }
        private IPipeServerStreamWrapper _pipeServerStreamWrapper;

        public event PipeMessageReceivedHandler MessageReceived;
        public event PipeResponseRequestedHandler ResponseRequested;

        public NamedPipeServer(PipeServerConfiguration configuration, IPipeServerStreamWrapper pipeServerStreamWrapper = null)
        {
            Configuration = configuration;
            _pipeServerStreamWrapper = pipeServerStreamWrapper;
        }

        /// <summary>
        /// Starts the named pipe server and listens for incoming messages.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task StartAsync()
        {
            while (!Configuration.CancellationTokenSource.IsCancellationRequested)
            {
                await Start();
            }
        }

        private async Task Start()
        {
            if (_pipeServerStreamWrapper == null)
            {
                var serverStream = new NamedPipeServerStream(
                    Configuration.PipeName,
                    PipeDirection.InOut,
                    1,
                    PipeTransmissionMode.Message,
                    PipeOptions.Asynchronous);

                _pipeServerStreamWrapper = new PipeServerStreamWrapper(serverStream);
            }

            using (_pipeServerStreamWrapper.ServerStream)
            {
                await HandlePipeStream();
            }
        }

        private async Task HandlePipeStream()
        {
            try
            {
                Configuration.Logger.LogInformation("Waiting for connection on pipe '{PipeName}'...", Configuration.PipeName);
                await _pipeServerStreamWrapper.WaitForConnectionAsync(Configuration.CancellationTokenSource.Token);
                Configuration.Logger.LogInformation("Client connected on '{PipeName}'", Configuration.PipeName);

                var buffer = new byte[4096];
                Configuration.Logger.LogInformation("Waiting for messages on pipe '{PipeName}'...", Configuration.PipeName);
                var bytesRead = await _pipeServerStreamWrapper.ReadAsync(buffer, 0, buffer.Length, Configuration.CancellationTokenSource.Token);
                await HandleMessage<dynamic>(bytesRead, buffer);
            }
            catch (Exception ex)
            {
                Configuration.Logger.LogTrace(ex, "Error occurred while handling pipe '{PipeName}'", Configuration.PipeName);

                var envelope = new PipeEnvelope<dynamic>
                {
                    Payload = $"Error handling pipe: {ex.Message}"
                };

                ResponseRequested?.Invoke(this, new PipeResponseRequestedEventArgs(
                    Guid.Empty,
                    envelope,
                    _pipeServerStreamWrapper.ServerStream));
            }
        }

        internal async Task HandleMessage<TTargetType>(int bytesRead, byte[] buffer)
        {
            if (bytesRead <= 0)
            {
                Configuration.Logger.LogInformation("No bytes read from pipe. Exiting message handling.");
                return;
            }

            try
            {
                var messageBytes = new byte[bytesRead];
                Array.Copy(buffer, 0, messageBytes, 0, bytesRead);

                var decryptedMessage = Configuration.EncryptionService.Decrypt(messageBytes);
                var message = Encoding.UTF8.GetString(decryptedMessage);
                var deserialized =
                    PipeEnvelope<TTargetType>.Deserialize<TTargetType>(message);

                Configuration.Logger.LogInformation($"Received message on pipe. Message Id: {deserialized.MessageId}");
                MessageReceived?.Invoke(this, new PipeMessageReceivedEventArgs(message, deserialized));

                var envelope = new PipeEnvelope<dynamic>
                {
                    Payload = $"Received message on pipe. Message Id: {deserialized.MessageId}"
                };

                ResponseRequested?
                    .Invoke(this,
                        new PipeResponseRequestedEventArgs(
                            deserialized.MessageId,
                            envelope,
                            _pipeServerStreamWrapper.ServerStream));

                await SendResponseAsync(envelope);
            }
            catch (Exception ex)
            {
                var errorMessage = "Error parsing message from pipe";
                Configuration.Logger.LogTrace(ex, errorMessage);

                var envelope = new PipeEnvelope<dynamic>
                {
                    Payload = $"{errorMessage}: {ex.Message}"
                };

                ResponseRequested?.Invoke(this,
                    new PipeResponseRequestedEventArgs(
                    Guid.Empty,
                    envelope,
                    _pipeServerStreamWrapper.ServerStream));
            }
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Configuration.CancellationTokenSource?.Cancel();
            MessageReceived = null;
            ResponseRequested = null;
            Configuration.Logger.LogInformation(
                $"Server disposed [{Configuration.ServerName} -> {Configuration.PipeName}]");
        }

        internal async Task SendResponseAsync(PipeEnvelope<dynamic> response)
        {
            if (_pipeServerStreamWrapper == null || !_pipeServerStreamWrapper.IsConnected)
            {
                return;
            }

            var json = response.ToJson();
            var bytes = Encoding.UTF8.GetBytes(json);

            if (Configuration.UseEncryption)
            {
                bytes = Configuration.EncryptionService.Encrypt(bytes);
            }

            await _pipeServerStreamWrapper.WriteAsync(bytes, 0, bytes.Length, Configuration.CancellationTokenSource.Token);
            await _pipeServerStreamWrapper.FlushAsync(Configuration.CancellationTokenSource.Token);
        }
    }
}
