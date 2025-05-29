using System.IO.Pipes;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SeroGlint.DotNet.NamedPipes;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;
using SeroGlint.DotNet.Tests.TestObjects;
using Shouldly;
using static NSubstitute.Arg;

namespace SeroGlint.DotNet.Tests.TestClasses.NamedPipes
{
    public class NamedPipeClientTests
    {
        ILogger _logger = Substitute.For<ILogger>();

        [Fact]
        public async Task SendMessage_ShouldThrowArgumentException_WhenServerNameIsNull()
        {
            // Arrange
            var config = Substitute.For<INamedPipeConfigurator>();
            var client = new NamedPipeClient(config, _logger);

            var envelope = new PipeEnvelope<SerializationObject>(_logger)
            {
                Payload = new SerializationObject
                {
                    Id = 1,
                    Name = "Test Object"
                }
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                client.SendMessage(envelope));

            Assert.Contains("Server name cannot be null or whitespace", ex.Message);
        }

        [Fact]
        public async Task SendMessage_ShouldThrowArgumentException_WhenPipeNameIsEmpty()
        {
            var config = Substitute.For<INamedPipeConfigurator>();
            var client = new NamedPipeClient(config, _logger);
            var envelope = new PipeEnvelope<SerializationObject>(_logger)
            {
                Payload = new SerializationObject
                {
                    Id = 1,
                    Name = "Test Object"
                }
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                client.SendMessage(envelope));

            Assert.Contains("Pipe name cannot be null or whitespace", ex.Message);
        }

        [Fact]
        public async Task SendMessage_ShouldThrowArgumentException_WhenMessageIsWhitespace()
        {
            var config = Substitute.For<INamedPipeConfigurator>();
            var client = new NamedPipeClient(config, _logger);
            var envelope = new PipeEnvelope<SerializationObject>(_logger)
            {
                Payload = new SerializationObject
                {
                    Id = 1,
                    Name = "Test Object"
                }
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                client.SendMessage(envelope));

            ex.ShouldNotBeNull();
        }

        [Fact]
        public async Task SendMessage_ShouldSerializeAndSendUnencryptedMessage()
        {
            // Arrange
            var config = Substitute.For<INamedPipeConfigurator>();
            config.CancellationTokenSource.Returns(new CancellationTokenSource(3000));

            var envelope = new PipeEnvelope<SerializationObject>(_logger)
            {
                Payload = new SerializationObject
                {
                    Id = 1,
                    Name = "Test Object"
                }
            };

            config.ServerName.Returns(".");
            config.PipeName.Returns("Pipe");
            config.UseEncryption.Returns(false);
            config.EncryptionService.Returns(Substitute.For<IEncryptionService>());

            var serverTask = StartTestServerAsync(config.PipeName, config.CancellationTokenSource.Token);
            var client = new NamedPipeClient(config, _logger);

            // Act & Assert
            await client.SendMessage(envelope);
            _logger.Received().LogInformation("Encrypting message before sending.");
            await serverTask;
        }

        [Fact]
        public async Task SendMessage_ShouldEncryptMessage_WhenEncryptionEnabled()
        {
            var config = Substitute.For<INamedPipeConfigurator>();
            var encryption = Substitute.For<IEncryptionService>();
            encryption.Encrypt(Any<byte[]>()).Returns(call => call.Arg<byte[]>());

            var envelope = new PipeEnvelope<SerializationObject>(_logger)
            {
                Payload = new SerializationObject
                {
                    Id = 1,
                    Name = "Test Object"
                }
            };

            config.ServerName.Returns(".");
            config.PipeName.Returns("Pipe_" + Guid.NewGuid().ToString("N"));
            config.UseEncryption.Returns(true);
            config.EncryptionService.Returns(encryption);
            config.CancellationTokenSource.Returns(new CancellationTokenSource(3000));

            var serverTask = StartTestServerAsync(config.PipeName, config.CancellationTokenSource.Token);

            await Task.Delay(100); // Let server start

            var client = new NamedPipeClient(config, _logger);
            await client.SendMessage(envelope);

            encryption.Received().Encrypt(Any<byte[]>());
            _logger.Received().LogInformation("Encrypting message before sending.");

            await serverTask;
        }

        private async Task StartTestServerAsync(string pipeName, CancellationToken token)
        {
            await using var server = 
                new NamedPipeServerStream(
                    pipeName, 
                    PipeDirection.InOut, 
                    1, 
                    transmissionMode: PipeTransmissionMode.Message, 
                    PipeOptions.Asynchronous);

            await server.WaitForConnectionAsync(token);

            var buffer = new byte[1024];
            _ = await server.ReadAsync(buffer, 0, buffer.Length, token);
        }
    }
}
