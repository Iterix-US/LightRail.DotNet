using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.NamedPipes
{
    public class NamedPipeClient : INamedPipeClient
    {
        private readonly ILogger _logger;
        private readonly IPipeMessageFormatter _formatter;

        public NamedPipeClient(IPipeMessageFormatter formatter, ILogger logger)
        {
            _logger = logger;
            _formatter = formatter;
        }

        public async Task SendMessage(string serverName, string pipeName, string messageContent, bool encryptMessage)
        {
            if (!ValidateMessageSettings(serverName, pipeName, messageContent))
            {
                return;
            }

            _logger.LogInformation("Encrypting message before sending.");
            await SendToPipeAsync(serverName, pipeName, messageContent, encryptMessage);
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

        private async Task SendToPipeAsync(string serverName, string pipeName, string messageContent, bool encrypt = true)
        {
            var message = 
                encrypt ?
                _formatter.Serialize(messageContent) :
                Encoding.UTF8.GetBytes(messageContent);

            using (var client =
                   new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                await client.ConnectAsync();
                await client.WriteAsync(message, 0, message.Length);
            }
        }
    }
}
