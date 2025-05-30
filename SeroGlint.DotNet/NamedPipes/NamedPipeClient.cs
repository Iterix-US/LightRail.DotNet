using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.NamedPipes
{
    /// <summary>
    /// A client for sending messages through a named pipe to a server.
    /// </summary>
    public class NamedPipeClient : INamedPipeClient
    {
        private readonly ILogger _logger;
        private readonly IPipeClientStreamWrapper _pipeClientStreamWrapper;

        internal INamedPipeConfigurator Configuration { get; private set; }

        public NamedPipeClient(INamedPipeConfigurator configuration, ILogger logger, IPipeClientStreamWrapper pipeClientStreamWrapper = null)
        {
            _logger = logger;
            Configuration = configuration;
            _pipeClientStreamWrapper = pipeClientStreamWrapper;
        }

        /// <summary>
        /// Sends a message through the named pipe to the server.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task SendMessage<T>(IPipeEnvelope<T> message)
        {
            if (!ValidateMessageSettings(Configuration.ServerName, Configuration.PipeName, message))
            {
                return;
            }

            _logger.LogInformation("Encrypting message before sending.");
            await SendToPipeAsync(message, Configuration);
        }

        internal bool ValidateMessageSettings<T>(string serverName, string pipeName, IPipeEnvelope<T> messageContent)
        {
            var stringBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(serverName))
            {
                const string errorMessage = "Server name cannot be null or whitespace.";
                stringBuilder.AppendLine(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(pipeName))
            {
                const string errorMessage = "Pipe name cannot be null or whitespace.";
                stringBuilder.AppendLine(errorMessage);
            }

            if (messageContent == null || messageContent.Payload == null || string.IsNullOrWhiteSpace(messageContent.Payload.ToJson()))
            {
                const string errorMessage = "Message content cannot be null or whitespace.";
                stringBuilder.AppendLine(errorMessage);
            }

            var errors = stringBuilder.ToString();
            if (errors.IsNullOrWhitespace())
            {
                return true;
            }

            _logger.LogError("Validation failed: {Errors}", errors);
            return false;
        }

        internal async Task SendToPipeAsync<T>(IPipeEnvelope<T> message, INamedPipeConfigurator configuration)
        {
            var serializedEnvelope = message.Serialize();
            if (configuration.UseEncryption)
            {
                _logger.LogInformation("Encrypting message before sending.");
                serializedEnvelope = configuration.EncryptionService.Encrypt(serializedEnvelope);
            }

            IPipeClientStreamWrapper client;
            if (_pipeClientStreamWrapper != null)
            {
                client = _pipeClientStreamWrapper;
                _logger.LogInformation($"Using injected pipe client stream wrapper with Id {client.Id}");
            }
            else
            {
                var clientStream = new NamedPipeClientStream(
                        configuration.ServerName,
                        configuration.PipeName,
                        PipeDirection.InOut,
                        PipeOptions.Asynchronous);

                client = new PipeClientStreamWrapper(clientStream);
                _logger.LogInformation("Instantiating new client base.");
            }
            _logger.LogInformation($"Using pipe client with Id {client.Id}");

            using (client)
            {
                await SendWithTimeout(client, serializedEnvelope);
            }
        }

        private async Task SendWithTimeout(IPipeClientStreamWrapper client, byte[] serializedEnvelope)
        {
            var token = Configuration.CancellationTokenSource.Token;
            await SendWithTimeoutInternal(client, serializedEnvelope, token);
        }

        // Virtual because our test project references this.
        internal virtual async Task SendWithTimeoutInternal(IPipeClientStreamWrapper client, byte[] serializedEnvelope, CancellationToken token)
        {
            try
            {
                await client.ConnectAsync(token);
                _logger.LogInformation("Connected to pipe '{PipeName}' on server '{ServerName}'", Configuration.PipeName, Configuration.ServerName);
                await client.WriteAsync(serializedEnvelope, 0, serializedEnvelope.Length, token);
                await client.FlushAsync(token);
                _logger.LogInformation("Message sent successfully to pipe '{PipeName}'. Client Id: {messageId}", Configuration.PipeName, client.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending message to pipe '{PipeName}'", Configuration.PipeName);
                throw new InvalidOperationException($"Failed to send message to pipe '{Configuration.PipeName}' on server '{Configuration.ServerName}'.", ex);
            }
        }

        public async Task<string> SendAndReceiveAsync<T>(IPipeEnvelope<T> message)
        {
            if (!ValidateMessageSettings(Configuration.ServerName, Configuration.PipeName, message))
            {
                return null;
            }

            var serializedEnvelope = message.Serialize();
            if (Configuration.UseEncryption)
            {
                _logger.LogInformation("Encrypting message before sending.");
                serializedEnvelope = Configuration.EncryptionService.Encrypt(serializedEnvelope);
            }

            var clientStream = new NamedPipeClientStream(
                Configuration.ServerName,
                Configuration.PipeName,
                PipeDirection.InOut,
                PipeOptions.Asynchronous);

            var client = new PipeClientStreamWrapper(clientStream);

            using (client)
            {
                try
                {
                    await client.ConnectAsync(Configuration.CancellationTokenSource.Token);
                    _logger.LogInformation("Connected to pipe '{PipeName}' on server '{ServerName}'", Configuration.PipeName, Configuration.ServerName);

                    await client.WriteAsync(serializedEnvelope, 0, serializedEnvelope.Length, Configuration.CancellationTokenSource.Token);
                    await client.FlushAsync(Configuration.CancellationTokenSource.Token);

                    var buffer = new byte[4096];
                    var bytesRead = await client.ReadAsync(buffer, 0, buffer.Length, Configuration.CancellationTokenSource.Token);
                    
                    if (bytesRead <= 0)
                    {
                        _logger.LogWarning("No response received from server on pipe '{PipeName}'", Configuration.PipeName);
                        return null;
                    }

                    var responseBytes = new byte[bytesRead];
                    Array.Copy(buffer, responseBytes, bytesRead);

                    if (Configuration.UseEncryption)
                    {
                        responseBytes = Configuration.EncryptionService.Decrypt(responseBytes);
                    }

                    var responseJson = Encoding.UTF8.GetString(responseBytes);
                    _logger.LogInformation("Received response from server: {Json}", responseJson);
                    return responseJson;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during SendAndReceiveAsync on pipe '{PipeName}'", Configuration.PipeName);
                    throw;
                }
            }

            return null;
        }

    }
}
