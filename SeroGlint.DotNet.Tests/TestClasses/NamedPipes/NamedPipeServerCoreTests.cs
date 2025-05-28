using NSubstitute;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.NamedPipes.Servers;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;
using System.IO.Pipes;
using System.Text;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using Xunit.Abstractions;
using SeroGlint.DotNet.Tests.TestObjects;

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

            var pipeServer = new NamedPipeServerCore(config);

            var messageReceived = false;
            pipeServer.MessageReceived += (_, e) =>
            {
                _testOutputHelper.WriteLine("MessageReceived triggered");
                logger.LogInformation("MessageReceived triggered");
                messageReceived = true;
            };

            var serializationObject = new SerializationObject
            {
                Id = 123456,
                Name = "TestObject",
            };

            var envelope = new PipeEnvelope<SerializationObject>
            {
                MessageId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                TypeName = typeof(SerializationObject).FullName,
                Payload = serializationObject
            };
            
            var json = envelope.ToJson();
            _testOutputHelper.WriteLine($"Serialized JSON: {json}");
            var decryptedBytes = Encoding.UTF8.GetBytes(json);
            _testOutputHelper.WriteLine($"Decrypted Bytes: {BitConverter.ToString(decryptedBytes)}");
            encryptionService.Decrypt(Arg.Any<byte[]>()).Returns(decryptedBytes);
            _testOutputHelper.WriteLine("Decryption service configured to return decrypted bytes.");

            // Start server
            var serverTask = Task.Run(pipeServer.StartAsync);
            await Task.Delay(200);
            _testOutputHelper.WriteLine("Server started, waiting for client to connect...");

            // Simulate client
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

            _testOutputHelper.WriteLine("Client disconnected, waiting for server to process message...");
            await Task.Delay(500); // allow message to be processed
            await config.CancellationTokenSource.CancelAsync();
            await serverTask;

            _testOutputHelper.WriteLine("Server task completed.");

            // Assert
            _testOutputHelper.WriteLine("Asserting that message was received...");
            _testOutputHelper.WriteLine($"Message received: {messageReceived}");
            Assert.True(messageReceived);
        }
    }
}