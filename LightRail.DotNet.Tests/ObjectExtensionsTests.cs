using LightRail.DotNet.Extensions;
using LightRail.DotNet.Tests.TestObjects;
using Shouldly;

namespace LightRail.DotNet.Tests
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void ToJson_ShouldSerializeObjectToJsonString()
        {
            // Arrange
            var testObject = new SerializationObject { Id = 1, Name = "Test" };
            var expectedJsonString = "{\"Id\":1,\"Name\":\"Test\"}";

            // Act
            var result = testObject.ToJson();

            // Assert
            result.ShouldBe(expectedJsonString);
        }

        [Fact]
        public void ToXml_ShouldThrowExceptionForInvalidObject()
        {
            var invalidObject = new InvalidObject();

            Should.Throw<InvalidOperationException>(() => invalidObject.ToXml())
                .Message.ShouldStartWith("Error serializing object");
        }
    }
}

