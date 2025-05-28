using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.NamedPipes.Packaging;

namespace SeroGlint.DotNet.NamedPipes
{
    public class NamedPipeClient : INamedPipeClient
    {
        private readonly ILogger _logger;
        private readonly INamedPipeConfigurator _configuration;

        public NamedPipeClient(INamedPipeConfigurator configuration, ILogger logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendMessage(string serverName, string pipeName, string messageContent, bool encryptMessage)
        {
            if (!ValidateMessageSettings(serverName, pipeName, messageContent))
            {
                return;
            }

            _logger.LogInformation("Encrypting message before sending.");
            await SendToPipeAsync(messageContent, _configuration);
        }

        private bool ValidateMessageSettings(string serverName, string pipeName, string messageContent)
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

            if (string.IsNullOrWhiteSpace(messageContent))
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

        private async Task SendToPipeAsync<TTargetType>(TTargetType message, INamedPipeConfigurator configuration)
        {
            var envelope = new PipeEnvelope<TTargetType>
            {
                Payload = message
            };

            var serializedEnvelope = envelope.Serialize();
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
                await client.ConnectAsync();
                await client.WriteAsync(serializedEnvelope, 0, serializedEnvelope.Length);
                await client.FlushAsync();
            }
        }
    }
}
