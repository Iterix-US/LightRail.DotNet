using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.Packaging;
using SeroGlint.DotNet.SecurityUtilities;
using SeroGlint.DotNet.Tests.TestObjects;

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

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                PipeEnvelope<SerializationObject>.Deserialize<SerializationObject>(json, logger));
            Assert.Equal("Failed to deserialize message.", ex.Message);
            logger.Received().LogWarning("Content submitted for deserialization is not a valid json package");
        }
    }
}
