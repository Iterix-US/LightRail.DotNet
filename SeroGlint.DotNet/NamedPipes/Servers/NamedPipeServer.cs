using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.Delegates;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;

namespace SeroGlint.DotNet.NamedPipes.Servers
{
    /// <summary>
    /// A named pipe server that handles incoming messages and processes them asynchronously.
    /// </summary>
    public class NamedPipeServer : INamedPipeServer
    {
        public PipeServerConfiguration Configuration { get; }

        public event PipeMessageReceivedHandler MessageReceived;
        public event PipeResponseRequestedHandler ResponseRequested;

        public NamedPipeServer(PipeServerConfiguration configuration)
        {
            Configuration = configuration;
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
            try
            {
                using (var server = new NamedPipeServerStream(
                           Configuration.PipeName,
                           PipeDirection.InOut,
                           1,
                           PipeTransmissionMode.Message,
                           PipeOptions.Asynchronous))
                {
                    await HandlePipeStream(server);
                }
            }
            catch (Exception ex)
            {
                Configuration.Logger.LogTrace(ex, "Error occurred while starting named pipe server");

                var envelope = new PipeEnvelope<dynamic>
                {
                    Payload = $"Error starting pipe server: {ex.Message}"
                };

                ResponseRequested?.Invoke(this, new PipeResponseRequestedEventArgs(
                    Guid.Empty,
                    envelope,
                    null));
            }
        }

        private async Task HandlePipeStream(NamedPipeServerStream server)
        {
            try
            {
                Configuration.Logger.LogInformation("Waiting for connection on pipe '{PipeName}'...", Configuration.PipeName);
                await server.WaitForConnectionAsync(Configuration.CancellationTokenSource.Token);
                Configuration.Logger.LogInformation("Client connected on '{PipeName}'", Configuration.PipeName);

                var buffer = new byte[4096];
                Configuration.Logger.LogInformation("Waiting for messages on pipe '{PipeName}'...", Configuration.PipeName);
                var bytesRead = await server.ReadAsync(buffer, 0, buffer.Length, Configuration.CancellationTokenSource.Token);
                if (bytesRead == 0)
                {
                    return;
                }

                await HandleMessage<dynamic>(bytesRead, buffer, server);
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
                    server));
            }
        }

        private async Task HandleMessage<TTargetType>(int bytesRead, byte[] buffer, NamedPipeServerStream server)
        {
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
                            server));

                await SendResponseAsync(envelope, server, Configuration.UseEncryption ? Configuration.EncryptionService : null);
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
                    server));
            }
        }

        [ExcludeFromCodeCoverage]
        public void Dispose() =>
            Configuration.Logger.LogInformation($"Server disposed [{Configuration.ServerName} -> {Configuration.PipeName}]");

        public static async Task SendResponseAsync(
            PipeEnvelope<dynamic> response,
            NamedPipeServerStream stream,
            IEncryptionService encryptionService = null)
        {
            if (stream == null || !stream.IsConnected)
                return;

            var json = response.ToJson();
            var bytes = Encoding.UTF8.GetBytes(json);

            if (encryptionService != null)
            {
                bytes = encryptionService.Encrypt(bytes);
            }

            await stream.WriteAsync(bytes, 0, bytes.Length);
            await stream.FlushAsync();
        }

    }
}
