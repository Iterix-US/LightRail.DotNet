using System.IO.Pipes;
using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.NamedPipes.Servers;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;
using SeroGlint.DotNet.Tests.TestObjects;
using Shouldly;
using Xunit.Abstractions;

namespace SeroGlint.DotNet.Tests.TestClasses.NamedPipes
{
    public class NamedPipeServerCoreTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public NamedPipeServerCoreTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task StartAsync_ShouldTriggerMessageReceived()
        {
            var logger = Substitute.For<ILogger>();
            var encryptionService = Substitute.For<IEncryptionService>();
            var pipeName = "TestPipe_" + Guid.NewGuid().ToString("N");

            var config = new PipeServerConfiguration
            {
                Logger = logger,
                EncryptionService = encryptionService,
                UseEncryption = true
            };
            config.SetPipeName(pipeName);
            config.SetServerName(".");

            var pipeServer = new NamedPipeServer(config);
            var messageReceived = false;

            pipeServer.MessageReceived += (_, _) =>
            {
                _testOutputHelper.WriteLine("MessageReceived triggered");
                logger.LogInformation("MessageReceived triggered");
                messageReceived = true;
            };

            var envelope = BuildTestPayload();
            var decryptedBytes = DecryptMessage(envelope, encryptionService);

            var serverTask = Task.Run(pipeServer.StartAsync);
            await Task.Delay(200);
            _testOutputHelper.WriteLine("Server started, waiting for client to connect...");

            await SimulateClient(pipeName, decryptedBytes);
            await CleanupServer(config, serverTask);

            _testOutputHelper.WriteLine("Asserting that message was received...");
            Assert.True(messageReceived);
        }

        [Fact]
        public async Task StartAsync_ShouldCatchGeneralException()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var encryptionService = Substitute.For<IEncryptionService>();
            _ = "TestPipe_" + Guid.NewGuid().ToString("N");

            var config = new PipeServerConfiguration
            {
                Logger = logger,
                EncryptionService = encryptionService,
                UseEncryption = true
            };
            config.SetServerName(".");

            // Inject null pipe name to force exception inside NamedPipeServerStream
            config.SetPipeName(null);

            var pipeServer = new NamedPipeServer(config);

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(pipeServer.StartAsync);

            // Assert
            ex.Message.ShouldContain("Error occurred while starting named pipe server.");
        }

        [Fact]
        public async Task StartAsync_ShouldThrowGenericException_WhenHandleMessageFails()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var encryptionService = Substitute.For<IEncryptionService>();
            var pipeName = "TestPipe_" + Guid.NewGuid().ToString("N");

            var config = new PipeServerConfiguration
            {
                Logger = logger,
                EncryptionService = encryptionService,
                UseEncryption = true
            };
            config.SetPipeName(pipeName);
            config.SetServerName(".");

            var pipeServer = new NamedPipeServer(config);

            encryptionService.Decrypt(Arg.Any<byte[]>()).Returns(_ => throw new Exception("Decryption failed"));

            Exception caught = null!;

            var serverTask = Task.Run(async () =>
            {
                try
                {
                    await pipeServer.StartAsync();
                }
                catch (Exception ex)
                {
                    caught = ex;
                }
            });

            await Task.Delay(200); // Let server spin up

            await using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
            {
                await client.ConnectAsync(2000);
                var bogusBytes = "{ \"not\": \"valid\" }"u8.ToArray();
                await client.WriteAsync(bogusBytes, 0, bogusBytes.Length);
                await client.FlushAsync();
            }

            await Task.Delay(300);
            await serverTask;

            // Assert
            Assert.NotNull(caught);
            Assert.IsType<Exception>(caught);
        }

        private async Task CleanupServer(PipeServerConfiguration config, Task serverTask)
        {
            _testOutputHelper.WriteLine("Client disconnected, waiting for server to process message...");
            await Task.Delay(500);

            await config.CancellationTokenSource.CancelAsync();

            try
            {
                await serverTask;
            }
            catch (OperationCanceledException ex)
            {
                _testOutputHelper.WriteLine("Server canceled as expected: " + ex.Message);
            }

            _testOutputHelper.WriteLine("Server task completed.");
        }

        private byte[] DecryptMessage(PipeEnvelope<SerializationObject> envelope, IEncryptionService encryptionService)
        {
            var json = envelope.ToJson();
            _testOutputHelper.WriteLine($"Serialized JSON: {json}");
            var decryptedBytes = Encoding.UTF8.GetBytes(json);
            _testOutputHelper.WriteLine($"Decrypted Bytes: {BitConverter.ToString(decryptedBytes)}");
            encryptionService.Decrypt(Arg.Any<byte[]>()).Returns(decryptedBytes);
            return decryptedBytes;
        }

        private static PipeEnvelope<SerializationObject> BuildTestPayload()
        {
            var serializationObject = new SerializationObject
            {
                Id = 123456,
                Name = "TestObject",
            };

            return new PipeEnvelope<SerializationObject>
            {
                MessageId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                TypeName = typeof(SerializationObject).FullName,
                Payload = serializationObject
            };
        }

        private async Task SimulateClient(string pipeName, byte[] decryptedBytes)
        {
            await using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
            {
                _testOutputHelper.WriteLine("Connecting client to server...");
                await client.ConnectAsync(2000);
                Assert.True(client.IsConnected, "Client failed to connect to server.");

                _testOutputHelper.WriteLine("Client connected, sending message...");
                await client.WriteAsync(decryptedBytes, 0, decryptedBytes.Length);
                await client.FlushAsync();
                _testOutputHelper.WriteLine("Message sent from client to server.");
            }
        }
    }
}