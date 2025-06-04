using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.Tests.TestObjects;
using Shouldly;

namespace SeroGlint.DotNet.Tests.TestClasses.NamedPipes
{
    public class PipeEnvelopeTests
    {
        [Fact]
        public void Serialize_ShouldReturnValidJsonBytes()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var envelope = new PipeEnvelope<SerializationObject>(logger)
            {
                Payload = new SerializationObject { Id = 1, Name = "Test" }
            };
            
            // Act
            var expectedBytes = Encoding.UTF8.GetBytes(envelope.ToJson());
            var serializedEnvelope = envelope.Serialize();

            // Assert
            Assert.Equal(expectedBytes, serializedEnvelope);
        }

        [Fact]
        public void Deserialize_ShouldReturnValidEnvelope()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var original = new PipeEnvelope<SerializationObject>
            {
                Payload = new SerializationObject { Id = 42, Name = "Demo" }
            };

            var json = original.ToJson();

            // Act
            var result = PipeEnvelope<SerializationObject>.Deserialize<SerializationObject>(json, logger);

            // Assert
            Assert.Equal(original.Payload.Id, result.Payload.Id);
            Assert.Equal(original.Payload.Name, result.Payload.Name);
            Assert.Equal(original.TypeName, result.TypeName);
        }

        [Fact]
        public void Deserialize_ShouldLogWarningOnInvalidJson()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var json = "Invalid JSON";
            var capturedMessage = string.Empty;
            logger
                .When(x => x.Log(
                    LogLevel.Error,
                    Arg.Any<EventId>(),
                    Arg.Do<object>(state => capturedMessage = state.ToString()),
                    Arg.Any<Exception>(),
                    Arg.Any<Func<object, Exception, string>>()!))
                .Do(_ => { });

            // Act & Assert
            var response = PipeEnvelope<SerializationObject>.Deserialize<SerializationObject>(json, logger);
            response.ShouldNotBeNull();
            capturedMessage.ShouldBeEquivalentTo("Failed to deserialize message.");
        }
    }
}
