using System.Text;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.Tests.TestObjects;
using Shouldly;

namespace SeroGlint.DotNet.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ContainsIgnoreCase_ShouldReturnTrue_WhenStringContainsValueIgnoringCase()
        {
            // Arrange
            const string baseString = "Hello World";
            const string value = "hello";

            // Act
            var result = baseString.ContainsIgnoreCase(value);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void EqualsIgnoreCase_ShouldReturnTrue_WhenStringsAreEqualIgnoringCase()
        {
            // Arrange
            const string baseString = "Hello";
            const string value = "hello";

            // Act
            var result = baseString.EqualsIgnoreCase(value);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void StartsWithIgnoreCase_ShouldReturnTrue_WhenStringStartsWithValueIgnoringCase()
        {
            // Arrange
            const string baseString = "Hello World";
            const string value = "hello";

            // Act
            var result = baseString.StartsWithIgnoreCase(value);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void EndsWithIgnoreCase_ShouldReturnTrue_WhenStringEndsWithValueIgnoringCase()
        {
            // Arrange
            const string baseString = "Hello World";
            const string value = "WORLD";

            // Act
            var result = baseString.EndsWithIgnoreCase(value);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void ToTitleCase_ShouldConvertStringToTitleCase()
        {
            // Arrange
            const string baseString = "hello world";

            // Act
            var result = baseString.ToTitleCase();

            // Assert
            result.ShouldBe("Hello World");
        }

        [Fact]
        public void ToInt32_ShouldConvertStringToInt32()
        {
            // Arrange
            const string baseString = "123";

            // Act
            var success = baseString.ToInt32(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBe(123);
        }

        [Fact]
        public void ToInt64_ShouldConvertStringToInt64()
        {
            // Arrange
            const string baseString = "123456789012345";

            // Act
            var success = baseString.ToInt64(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBe(123456789012345);
        }

        [Fact]
        public void ToDouble_ShouldConvertStringToDouble()
        {
            // Arrange
            const string baseString = "123.45";

            // Act
            var success = baseString.ToDouble(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBe(123.45);
        }

        [Fact]
        public void ToDecimal_ShouldConvertStringToDecimal()
        {
            // Arrange
            const string baseString = "123.45";

            // Act
            var success = baseString.ToDecimal(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBe(123.45m);
        }

        [Fact]
        public void ToDateTime_ShouldConvertStringToDateTime()
        {
            // Arrange
            const string baseString = "2025-03-06";

            // Act
            var success = baseString.ToDateTime(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBe(new DateTime(2025, 3, 6));
        }

        [Fact]
        public void ToBoolean_ShouldConvertStringToTrueBoolean()
        {
            // Arrange
            const string baseString = "true";

            // Act
            var success = baseString.ToBoolean(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBeTrue();
        }

        [Fact]
        public void ToBoolean_ShouldConvertStringToFalseBoolean()
        {
            // Arrange
            const string baseString = "false";

            // Act
            var success = baseString.ToBoolean(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBeFalse();
        }

        [Fact]
        public void ToBoolean_ShouldConvertStringTo1TrueBoolean()
        {
            // Arrange
            const string baseString = "1";

            // Act
            var success = baseString.ToBoolean(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBeTrue();
        }

        [Fact]
        public void ToBoolean_ShouldConvertStringTo0FalseBoolean()
        {
            // Arrange
            const string baseString = "0";

            // Act
            var success = baseString.ToBoolean(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBeFalse();
        }

        [Fact]
        public void FromJsonToType_ShouldDeserializeJsonStringToObject()
        {
            // Arrange
            const string jsonString = "{\"Id\":1,\"Name\":\"Test\"}";
            var expectedObject = new SerializationObject { Id = 1, Name = "Test" };

            // Act
            var result = jsonString.FromJsonToType<SerializationObject>();

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(expectedObject.Id);
            result.Name.ShouldBe(expectedObject.Name);
        }

        [Fact]
        public void FromJsonToType_ShouldThrowExceptionForInvalidJson()
        {
            // Arrange
            const string invalidJsonString = "{\"Id\":1,\"Name\":\"Test\"";

            // Act & Assert
            var exception = Should.Throw<Exception>(() => invalidJsonString.FromJsonToType<SerializationObject>());
            exception.Message.ShouldContain("An error occurred while deserializing the JSON string.");
        }

        [Fact]
        public void FromXmlToType_ShouldDeserializeXmlStringToObject()
        {
            // Arrange
            const string xmlString = "<SerializationObject><Id>1</Id><Name>Test</Name></SerializationObject>";
            var expectedObject = new SerializationObject { Id = 1, Name = "Test" };

            // Act
            var result = xmlString.FromXmlToType<SerializationObject>();

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(expectedObject.Id);
            result.Name.ShouldBe(expectedObject.Name);
        }

        [Fact]
        public void FromXmlToType_ShouldThrowExceptionForInvalidXml()
        {
            // Arrange
            const string invalidXmlString = "<SerializationObject><Id>1<Id><Name>Test</Name></SerializationObject>";

            // Act & Assert
            var exception = Should.Throw<InvalidOperationException>(() => invalidXmlString.FromXmlToType<SerializationObject>());
            exception.Message.ShouldContain($"Error deserializing XML to {typeof(SerializationObject)}.");
        }
        
        [Fact]
        public void IsValidEnumValue_ShouldReturnTrue_WhenValidEnumString()
        {
            const string input = "ValueOne";
            var success = input.IsValidEnumValue<SampleEnum>(out var result);
    
            success.ShouldBeTrue();
            result.ShouldBe(SampleEnum.ValueOne);
        }

        [Fact]
        public void IsValidEnumValue_ShouldReturnFalse_WhenInvalidEnumString()
        {
            const string input = "Invalid";
            var success = input.IsValidEnumValue<SampleEnum>(out _);
    
            success.ShouldBeFalse();
        }

        [Fact]
        public void GetEnumFromDescription_ShouldReturnEnum_WhenDescriptionMatches()
        {
            const string input = "First Option";
            var result = input.GetEnumFromDescription<SampleEnum>();

            result.ShouldBe(SampleEnum.ValueOne);
        }

        [Fact]
        public void GetEnumFromDescription_ShouldReturnNull_WhenNoMatch()
        {
            const string input = "Nonexistent";
            var result = input.GetEnumFromDescription<SampleEnum>();

            result.ShouldBeNull();
        }

        [Fact]
        public void EncodeAsHttp_ShouldUrlEncodeString()
        {
            const string input = "this is a test!";
            var result = input.EncodeAsHttp();

            result.ShouldBe("this%20is%20a%20test%21");
        }

        [Fact]
        public void ToSha256_ShouldReturnExpectedHash()
        {
            const string input = "test";
            var result = input.ToSha256();

            result.ShouldBe("n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=");
        }

        [Fact]
        public void ToHexString_ShouldConvertStringToHex()
        {
            const string input = "abc";
            var result = input.ToHexString();

            result.ShouldBe("616263");
        }

        [Fact]
        public void ToHexString_ShouldRespectEncodingParameter()
        {
            const string input = "abc";
            var result = input.ToHexString(Encoding.ASCII);

            result.ShouldBe("616263");
        }
    }
}
