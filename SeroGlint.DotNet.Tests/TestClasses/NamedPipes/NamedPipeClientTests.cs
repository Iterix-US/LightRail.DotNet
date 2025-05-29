using System.IO.Pipes;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SeroGlint.DotNet.NamedPipes;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;
using SeroGlint.DotNet.Tests.TestClasses.NamedPipes.TestObjects;
using SeroGlint.DotNet.Tests.TestObjects;
using SeroGlint.DotNet.Tests.Utilities;
using static NSubstitute.Arg;
#pragma warning disable CA1416

namespace SeroGlint.DotNet.Tests.TestClasses.NamedPipes
{
    public class NamedPipeClientTests
    {
        private readonly ILogger _logger = Substitute.For<ILogger>();

        [Fact]
        public async Task SendMessage_ShouldLogError_WhenServerNameIsNull()
        {
            // Arrange
            var config = Substitute.For<INamedPipeConfigurator>();
            config.ServerName.Returns((string)null!);
            config.PipeName.Returns("PipeName");
            config.CancellationTokenSource.Returns(new CancellationTokenSource(3000));
            config.EncryptionService.Returns(Substitute.For<IEncryptionService>());

            var capturedMessage = string.Empty;

            _logger
                .When(x => x.Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Do<object>(state => capturedMessage = state.ToString()),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<object, Exception, string>>()!))
                .Do(_ => { });

            var envelope = new PipeEnvelope<SerializationObject>(_logger)
            {
                Payload = new SerializationObject
                {
                    Id = 1,
                    Name = "Test Object"
                }
            };

            var client = new NamedPipeClient(config, _logger);

            // Act
            await client.SendMessage(envelope);

            // Assert
            Assert.Contains("Server name cannot be null or whitespace", capturedMessage);
        }

        [Fact]
        public async Task SendMessage_ShouldLogError_WhenPipeNameIsEmpty()
        {
            // Arrange
            var config = Substitute.For<INamedPipeConfigurator>();
            config.ServerName.Returns("ValidServer");
            config.PipeName.Returns(""); // Invalid pipe name
            config.CancellationTokenSource.Returns(new CancellationTokenSource(3000));
            config.EncryptionService.Returns(Substitute.For<IEncryptionService>());

            var capturedMessage = string.Empty;
            _logger
                .When(x => x.Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Do<object>(state => capturedMessage = state.ToString()),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<object, Exception, string>>()!))
                .Do(_ => { });

            var envelope = new PipeEnvelope<SerializationObject>(_logger)
            {
                Payload = new SerializationObject { Id = 1, Name = "Test" }
            };

            var client = new NamedPipeClient(config, _logger);

            // Act
            await client.SendMessage(envelope);

            // Assert
            Assert.Contains("Pipe name cannot be null or whitespace", capturedMessage);
        }


        [Fact]
        public async Task SendWithTimeout_ShouldThrowWrappedException_WhenWriteFails()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var config = Substitute.For<INamedPipeConfigurator>();
            var pipeWrapper = Substitute.For<IPipeClientStreamWrapper>();
            var envelope = Substitute.For<IPipeEnvelope<SerializationObject>>();

            var payload = new SerializationObject { Id = 1, Name = "Failure" };
            envelope.Payload.Returns(payload);
            envelope.Serialize().Returns(new byte[] { 1, 2, 3, 4 });

            config.ServerName.Returns("TestServer");
            config.PipeName.Returns("TestPipe");
            config.CancellationTokenSource.Returns(new CancellationTokenSource(1000));
            config.UseEncryption.Returns(false);
            config.EncryptionService.Returns(Substitute.For<IEncryptionService>());

            pipeWrapper
                .When(x => x.WriteAsync(Arg.Any<byte[]>(), 0, 4, Arg.Any<CancellationToken>()))
                .Do(_ => throw new IOException("Simulated write failure"));

            var client = new TestableNamedPipeClient(config, logger, pipeWrapper);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => client.SendMessage(envelope));
            Assert.Contains("Failed to send message to pipe", ex.Message);
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

            var serverTask = TestNamedPipeServer.StartTestServerAsync(config.PipeName, config.CancellationTokenSource.Token);
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

            var serverTask = TestNamedPipeServer.StartTestServerAsync(config.PipeName, config.CancellationTokenSource.Token);

            await Task.Delay(100); // Let server start

            var client = new NamedPipeClient(config, _logger);
            await client.SendMessage(envelope);

            encryption.Received().Encrypt(Any<byte[]>());
            _logger.Received().LogInformation("Encrypting message before sending.");

            await serverTask;
        }

        [Fact]
        public async Task SendMessage_ShouldFailValidationWhenInvalidServerInformationPassedIn()
        {
            // Arrange
            var capturedMessage = string.Empty;

            var config = Substitute.For<INamedPipeConfigurator>();
            config.CancellationTokenSource.Returns(new CancellationTokenSource(3000));

            var envelope = new PipeEnvelope<SerializationObject>(_logger);

            config.ServerName.Returns("");
            config.PipeName.Returns("");
            config.UseEncryption.Returns(false);
            config.EncryptionService.Returns(Substitute.For<IEncryptionService>());

            _logger
                .When(x => x.Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Do<object>(state => capturedMessage = state.ToString()),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<object, Exception, string>>()!))
                .Do(_ => { });

            var client = new NamedPipeClient(config, _logger);

            // Act
            await client.SendMessage(envelope);

            // Assert
            Assert.Contains("Server name cannot be null or whitespace", capturedMessage);
            Assert.Contains("Pipe name cannot be null or whitespace", capturedMessage);
            Assert.Contains("Message content cannot be null or whitespace", capturedMessage);
        }
    }
}
