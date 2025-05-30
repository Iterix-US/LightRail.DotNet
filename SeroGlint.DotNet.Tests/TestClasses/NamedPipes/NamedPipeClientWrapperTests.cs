using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SeroGlint.DotNet.NamedPipes;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.Tests.TestClasses.NamedPipes.TestObjects;
using SeroGlint.DotNet.Tests.TestObjects;
using SeroGlint.DotNet.Tests.Utilities;

namespace SeroGlint.DotNet.Tests.TestClasses.NamedPipes
{
    public partial class NamedPipeClientWrapperTests
    {
        [Fact]
        public async Task SendWithTimeout_ShouldThrowWrappedException_WhenWriteFails()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var config = Substitute.For<INamedPipeConfigurator>();
            var pipeWrapper = Substitute.For<IPipeClientStreamWrapper>();
            var envelope = Substitute.For<IPipeEnvelope<SerializationObject>>();

            var testPayload = new byte[] { 1, 2, 3, 4 };
            envelope.Serialize().Returns(testPayload);
            envelope.Payload.Returns(new SerializationObject { Id = -1, Name = null! });

            config.ServerName.Returns("TestServer");
            config.PipeName.Returns("TestPipe");
            config.CancellationTokenSource.Returns(new CancellationTokenSource(1000));
            config.UseEncryption.Returns(false);

            pipeWrapper
                .When(x => x.WriteAsync(Arg.Any<byte[]>(), 0, 4, Arg.Any<CancellationToken>()))
                .Do(_ => throw new IOException("Simulated write failure"));

            var client = new TestableNamedPipeClient(config, logger, pipeWrapper);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => client.SendMessage(envelope));
            Assert.Contains("Failed to send message to pipe", ex.Message);
        }

        [Fact]
        public async Task SendWithTimeout_ShouldUseInjectedWrapper_WhenWrapperNotNull()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var config = Substitute.For<INamedPipeConfigurator>();
            var pipeWrapper = Substitute.For<IPipeClientStreamWrapper>();
            var envelope = Substitute.For<IPipeEnvelope<SerializationObject>>();

            var testPayload = new byte[] { 1, 2, 3, 4 };
            var testId = Guid.NewGuid();

            envelope.Serialize().Returns(testPayload);
            envelope.Payload.Returns(new SerializationObject { Id = 1, Name = "something" });

            config.ServerName.Returns(".");
            config.PipeName.Returns("TestPipe");
            config.CancellationTokenSource.Returns(new CancellationTokenSource(3000));
            config.UseEncryption.Returns(false);

            pipeWrapper.Id.Returns(testId);
            pipeWrapper.ConnectAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            pipeWrapper.WriteAsync(Arg.Any<byte[]>(), 0, 4, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            pipeWrapper.FlushAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            var client = new TestableNamedPipeClient(config, logger, pipeWrapper);

            // Act
            string? captured = null;
            logger
                .When(x => x.Log(
                    LogLevel.Information,
                    Arg.Any<EventId>(),
                    Arg.Do<object>(state => captured = state.ToString()),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<object, Exception, string>>()!))
                .Do(_ => { });

            await client.SendMessage(envelope);

            // Assert
            Assert.NotNull(captured);
            Assert.Contains(testId.ToString(), captured);
        }

        [Fact]
        public async Task SendToPipeAsync_ShouldUseInjectedWrapper()
        {
            var logger = Substitute.For<ILogger>();
            var config = Substitute.For<INamedPipeConfigurator>();
            var envelope = Substitute.For<IPipeEnvelope<SerializationObject>>();
            var wrapper = Substitute.For<IPipeClientStreamWrapper>();

            var testId = Guid.NewGuid();
            var payload = new byte[] { 0x1, 0x2 };

            config.ServerName.Returns("TestHost");
            config.PipeName.Returns("TestPipe");
            config.UseEncryption.Returns(false);
            config.CancellationTokenSource.Returns(new CancellationTokenSource(1000));

            wrapper.Id.Returns(testId);
            wrapper.ConnectAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            wrapper.WriteAsync(Arg.Any<byte[]>(), 0, 2, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            wrapper.FlushAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            envelope.Payload.Returns(new SerializationObject { Id = 1, Name = "data" });
            envelope.Serialize().Returns(payload);

            var client = new NamedPipeClient(config, logger, wrapper);

            // Act
            string? captured = null;
            logger
                .When(x => x.Log(
                    LogLevel.Information,
                    Arg.Any<EventId>(),
                    Arg.Do<object>(state => captured = state.ToString()),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<object, Exception, string>>()!))
                .Do(_ => { });

            await client.SendMessage(envelope);

            // Assert
            Assert.NotNull(captured);
            Assert.Contains(testId.ToString(), captured);
        }


        [Fact]
        public async Task SendWithTimeout_ShouldUseNewWrapper_WhenNoWrapperInjected()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var config = Substitute.For<INamedPipeConfigurator>();
            var envelope = Substitute.For<IPipeEnvelope<SerializationObject>>();

            var testPayload = new byte[] { 1, 2, 3, 4 };

            envelope.Serialize().Returns(testPayload);
            envelope.Payload.Returns(new SerializationObject { Id = 1, Name = "something" });

            config.ServerName.Returns(".");
            config.PipeName.Returns("TestPipe");
            config.CancellationTokenSource.Returns(new CancellationTokenSource(5000));
            config.UseEncryption.Returns(false);

            var client = new TestableNamedPipeClient(config, logger);

            var serverReady = new TaskCompletionSource();
            var serverTask = TestNamedPipeServer.StartTestServerAsync(config.PipeName, config.CancellationTokenSource.Token, serverReady);
            await serverReady.Task;

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => client.SendToPipeAsync(envelope, config));

            Assert.Null(ex.Exception);

            await serverTask;
        }
    }
}
