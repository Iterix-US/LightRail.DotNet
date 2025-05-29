using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.NamedPipes.Delegates;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.NamedPipes.Packaging;

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
            catch (OperationCanceledException ex)
            {
                Configuration.Logger.LogInformation("Operation cancelled on pipe '{PipeName}'", Configuration.PipeName);
                throw new OperationCanceledException("Pipe operation was cancelled.", ex);
            }
            catch (Exception ex)
            {
                Configuration.Logger.LogError(ex, "Error occurred while starting named pipe server");
                ResponseRequested?.Invoke(this, new PipeResponseRequestedEventArgs(
                    Guid.Empty,
                    $"Error starting pipe server: {ex.Message}",
                    null));

                throw new Exception("Error occurred while starting named pipe server.", ex);
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
                while (server.IsConnected || !Configuration.CancellationTokenSource.IsCancellationRequested)
                {
                    Configuration.Logger.LogInformation("Waiting for messages on pipe '{PipeName}'...", Configuration.PipeName);
                    var bytesRead = await server.ReadAsync(buffer, 0, buffer.Length, Configuration.CancellationTokenSource.Token);
                    if (bytesRead == 0)
                    {
                        continue;
                    }

                    HandleMessage<dynamic>(bytesRead, buffer, server);
                }
            }
            catch (OperationCanceledException ex)
            {
                Configuration.Logger.LogInformation("Operation cancelled on pipe '{PipeName}'", Configuration.PipeName);
                ResponseRequested?.Invoke(this, new PipeResponseRequestedEventArgs(
                    Guid.Empty,
                    $"Error handling pipe: {ex.Message}",
                    server));
                throw new OperationCanceledException("Pipe operation was cancelled.", ex);
            }
            catch (Exception ex)
            {
                Configuration.Logger.LogError(ex, "Error occurred while handling pipe '{PipeName}'", Configuration.PipeName);
                ResponseRequested?.Invoke(this, new PipeResponseRequestedEventArgs(
                    Guid.Empty,
                    $"Error handling pipe: {ex.Message}",
                    server));
                throw new Exception("Error occurred while handling pipe operation.");
            }
        }

        private void HandleMessage<TTargetType>(int bytesRead, byte[] buffer, NamedPipeServerStream server)
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
                ResponseRequested?
                    .Invoke(this,
                        new PipeResponseRequestedEventArgs(
                            deserialized.MessageId,
                            $"Message received. Id: {deserialized.MessageId}",
                            server));
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to parse message from pipe";
                Configuration.Logger.LogTrace(ex, errorMessage);
                ResponseRequested?.Invoke(this,
                    new PipeResponseRequestedEventArgs(
                    Guid.Empty,
                    errorMessage,
                    server));

                throw new Exception("Pipe message handling failed", ex);
            }
        }

        [ExcludeFromCodeCoverage]
        public void Dispose() => 
            Configuration.Logger.LogInformation($"Server disposed [{Configuration.ServerName} -> {Configuration.PipeName}]");
    }
}
