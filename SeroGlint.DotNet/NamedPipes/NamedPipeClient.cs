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
        private readonly IPipeClientStreamWrapper _pipeClientStreamWrapper;

        internal INamedPipeConfigurator Configuration { get; }

        public NamedPipeClient(INamedPipeConfigurator configuration, ILogger logger,
            IPipeClientStreamWrapper pipeClientStreamWrapper = null)
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
        public virtual async Task<string> Send<T>(IPipeEnvelope<T> message)
        {
            if (!ValidateMessageSettings(Configuration.ServerName, Configuration.PipeName, message))
            {
                return "Pipe message was invalid. Check log for more details.";
            }

            var serializedEnvelope = PrepareMessage(message);
            var client = EstablishClientStream();

            using (client)
            {
                var sendResult = await SendThroughPipe(client, serializedEnvelope);

                if (sendResult.IsNullOrWhitespace())
                {
                    return sendResult;
                }

                return await ParseResponse(client);
            }
        }

        private async Task<string> ParseResponse(IPipeClientStreamWrapper client)
        {
            try
            {
                var responseBytes = await RetrieveResponse(client);
                var responseJson = Encoding.UTF8.GetString(responseBytes);

                _logger.LogInformation(
                    $"Client ({client.Id}) received response from server ({Configuration.ServerName}) on pipe ({Configuration.PipeName})");
                return responseJson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while reading response from pipe {Configuration.PipeName}");
                return $"Error reading response: {ex.Message}";
            }
        }

        private async Task<string> SendThroughPipe(IPipeClientStreamWrapper client, byte[] serializedEnvelope)
        {
            try
            {
                await WriteToStream(client, serializedEnvelope);
                return "Message sent.";
            }
            catch (Exception ex)
            {
                var message = $"Error occurred sending on pipe {Configuration.PipeName}";
                _logger.LogError(ex, message);
                return $"{message}: {ex.Message}";
            }
        }

        private async Task<byte[]> RetrieveResponse(IPipeClientStreamWrapper client)
        {
            var buffer = new byte[4096];
            var bytesRead = await client.ClientStream.ReadAsync(buffer, 0, buffer.Length, Configuration.CancellationTokenSource.Token);
            var responseBytes = new byte[bytesRead];

            if (bytesRead <= 0)
            {
                _logger.LogWarning($"No response received from server on pipe {Configuration.PipeName}");
                return responseBytes;
            }

            Array.Copy(buffer, responseBytes, bytesRead);

            if (Configuration.UseEncryption)
            {
                responseBytes = Configuration.EncryptionService.Decrypt(responseBytes);
            }

            return responseBytes;
        }

        private async Task WriteToStream(IPipeClientStreamWrapper client, byte[] serializedEnvelope)
        {
            await client.ConnectAsync(Configuration.CancellationTokenSource.Token);
            _logger.LogInformation($"Connected to pipe {Configuration.PipeName} on server {Configuration.ServerName}");

            await client.WriteAsync(serializedEnvelope, 0, serializedEnvelope.Length, Configuration.CancellationTokenSource.Token);
            await client.FlushAsync(Configuration.CancellationTokenSource.Token);
        }

        private byte[] PrepareMessage<T>(IPipeEnvelope<T> message)
        {
            var serializedEnvelope = message.Serialize();
            
            if (!Configuration.UseEncryption)
            {
                return serializedEnvelope;
            }

            _logger.LogInformation("Encrypting message before sending.");
            serializedEnvelope = Configuration.EncryptionService.Encrypt(serializedEnvelope);

            return serializedEnvelope;
        }

        private PipeClientStreamWrapper EstablishClientStream()
        {
            if (_pipeClientStreamWrapper != null)
            {
                _logger.LogInformation("Using provided PipeClientStreamWrapper.");
                return _pipeClientStreamWrapper as PipeClientStreamWrapper;
            }

            var clientStream = new NamedPipeClientStream(
                Configuration.ServerName,
                Configuration.PipeName,
                PipeDirection.InOut,
                PipeOptions.Asynchronous);

            var client = new PipeClientStreamWrapper(clientStream);
            _logger.LogInformation($"Created new NamedPipeClientStream for server '{Configuration.ServerName}' and pipe '{Configuration.PipeName}'");
            return client;
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

            _logger.LogError($"Validation failed: {errors}");
            return false;
        }
    }
}
