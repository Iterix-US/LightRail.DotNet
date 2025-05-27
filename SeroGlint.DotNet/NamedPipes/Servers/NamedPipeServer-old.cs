using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.NamedPipes
{
    public class NamedPipeServer : INamedPipeServer
    {
        private readonly ILogger _logger;
        private readonly PipeConfiguration _config;
        private readonly IPipeMessageFormatter _formatter;
        private NamedPipeServerStream _server;
        
        public event PipeMessageReceivedHandler MessageReceived;

        public NamedPipeServer(PipeConfiguration config, ILogger logger)
        {
            _logger = logger;
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _formatter = config.Formatter ?? new EncryptedJsonMessageFormatter(config.EncryptionKey, _logger);
        }

        public void Dispose()
        {
            _logger.LogInformation("NamedPipeServer disposed.");
        }

        public void InitializeServer(string name)
        {
            if (name.IsNullOrWhitespace())
            {
                throw new ArgumentException("Pipe name cannot be null or whitespace.", nameof(name));
            }

            if (_config.ServerMode == PipeServerMode.Perpetual)
            {
                _ = InitializePerpetualServer(name);
                return;
            }
         
            InitializeMomentaryServer(name);
        }

        private async Task InitializePerpetualServer(string name)
        {
            _logger.LogInformation("Starting perpetual named pipe server on '{Name}'...", name);

            while (true)
            {
                using (_server =
                           new NamedPipeServerStream(
                               name,
                               PipeDirection.InOut, 1,
                               PipeTransmissionMode.Message,
                               PipeOptions.Asynchronous))
                {
                    await HandleServerInitialization(name);
                }
            }
        }

        private void InitializeMomentaryServer(string name)
        {
            using (var server = new NamedPipeServerStream(name, PipeDirection.InOut, 1, PipeTransmissionMode.Message,
                       PipeOptions.Asynchronous))
            {
                _logger.LogInformation("Named pipe server '{Name}' initialized (momentary).", name);
                server.WaitForConnection();
                _logger.LogInformation("Client connected to named pipe server '{Name}'.", name);

                var buffer = new byte[4096];
                var bytesRead = server.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    return;
                }

                var messageBytes = new byte[bytesRead];
                Array.Copy(buffer, messageBytes, bytesRead);

                var message = _formatter.Deserialize<object>(messageBytes);
                _logger.LogInformation("Received (momentary): {Message}", message.ToJson());
            }
        }

        private void GH()
        {
            var buffer = new byte[4096];

            if (_config.ServerMode == PipeServerMode.Perpetual)
            {

            }
        }

        private async Task HandleServerInitialization(string name)
        {
            await _server.WaitForConnectionAsync();
            _logger.LogInformation("Client connected to named pipe server '{Name}' (perpetual).", name);

            try
            {
                var buffer = new byte[4096];
                while (_server.IsConnected)
                {
                    var bytesRead = await _server.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) continue;

                    var messageBytes = new byte[bytesRead];
                    Array.Copy(buffer, 0, messageBytes, 0, bytesRead);

                    var message = _formatter.Deserialize<object>(messageBytes);
                    _logger.LogInformation("Received (perpetual): {Message}", message.ToJson());

                    var response = _formatter.Serialize("Message received.");
                    await _server.WriteAsync(response, 0, response.Length);
                }
            }
            catch (IOException ex)
            {
                _logger.LogWarning(ex, "Client disconnected or pipe closed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in perpetual pipe server.");
            }
        }

        public void CloseServer()
        {
            if (_server.IsConnected)
            {
                _server.Disconnect();
                _logger.LogInformation("Named pipe server closed.");
            }

            Dispose();
        }
    }
}
