using System;
using System.IO.Pipes;
using System.Text;
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
        private readonly INamedPipeConfigurator _configuration;

        public NamedPipeClient(INamedPipeConfigurator configuration, ILogger logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Sends a message through the named pipe to the server.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage<T>(IPipeEnvelope<T> message)
        {
            if (!ValidateMessageSettings(_configuration.ServerName, _configuration.PipeName, message))
            {
                return;
            }

            _logger.LogInformation("Encrypting message before sending.");
            await SendToPipeAsync(message, _configuration);
        }

        private bool ValidateMessageSettings<T>(string serverName, string pipeName, IPipeEnvelope<T> messageContent)
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

            if (string.IsNullOrWhiteSpace(messageContent.Payload.ToJson()))
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
            throw new ArgumentException(errors);
        }

        private async Task SendToPipeAsync<T>(IPipeEnvelope<T> message, INamedPipeConfigurator configuration)
        {
            var serializedEnvelope = message.Serialize();
            if (configuration.UseEncryption)
            {
                _logger.LogInformation("Encrypting message before sending.");
                serializedEnvelope = configuration.EncryptionService.Encrypt(serializedEnvelope);
            }

            using (var client = new NamedPipeClientStream(
                       configuration.ServerName, 
                       configuration.PipeName, 
                       PipeDirection.InOut,
                       PipeOptions.Asynchronous))
            {
                await SendWithTimeout(client, serializedEnvelope);
            }
        }

        private async Task SendWithTimeout(NamedPipeClientStream client, byte[] serializedEnvelope)
        {
            var token = _configuration.CancellationTokenSource.Token;
            try
            {
                await client.ConnectAsync(token);
                _logger.LogInformation("Connected to pipe '{PipeName}' on server '{ServerName}'",
                    _configuration.PipeName, _configuration.ServerName);
                await client.WriteAsync(serializedEnvelope, 0, serializedEnvelope.Length, token);
                _logger.LogInformation("Message sent successfully to pipe '{PipeName}'", _configuration.PipeName);
                await client.FlushAsync(token);
                _logger.LogInformation("Flush completed for pipe '{PipeName}'", _configuration.PipeName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending message to pipe '{PipeName}'", _configuration.PipeName);
                throw new InvalidOperationException($"Failed to send message to pipe '{_configuration.PipeName}' on server '{_configuration.ServerName}'.", ex);
            }
        }
    }
}
