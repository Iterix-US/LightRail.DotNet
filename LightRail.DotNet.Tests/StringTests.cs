using LightRail.DotNet.Extensions;
using Shouldly;

namespace LightRail.DotNet.Tests
{
    public class StringTests
    {
        [Fact]
        public void ContainsIgnoreCase_ShouldReturnTrue_WhenStringContainsValueIgnoringCase()
        {
            // Arrange
            var baseString = "Hello World";
            var value = "hello";

            // Act
            var result = baseString.ContainsIgnoreCase(value);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void EqualsIgnoreCase_ShouldReturnTrue_WhenStringsAreEqualIgnoringCase()
        {
            // Arrange
            var baseString = "Hello";
            var value = "hello";

            // Act
            var result = baseString.EqualsIgnoreCase(value);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void StartsWithIgnoreCase_ShouldReturnTrue_WhenStringStartsWithValueIgnoringCase()
        {
            // Arrange
            var baseString = "Hello World";
            var value = "hello";

            // Act
            var result = baseString.StartsWithIgnoreCase(value);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void EndsWithIgnoreCase_ShouldReturnTrue_WhenStringEndsWithValueIgnoringCase()
        {
            // Arrange
            var baseString = "Hello World";
            var value = "WORLD";

            // Act
            var result = baseString.EndsWithIgnoreCase(value);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void ToTitleCase_ShouldConvertStringToTitleCase()
        {
            // Arrange
            var baseString = "hello world";

            // Act
            var result = baseString.ToTitleCase();

            // Assert
            result.ShouldBe("Hello World");
        }

        [Fact]
        public void ToInt32_ShouldConvertStringToInt32()
        {
            // Arrange
            var baseString = "123";

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
            var baseString = "123456789012345";

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
            var baseString = "123.45";

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
            var baseString = "123.45";

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
            var baseString = "2025-03-06";

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
            var baseString = "true";

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
            var baseString = "false";

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
            var baseString = "1";

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
            var baseString = "0";

            // Act
            var success = baseString.ToBoolean(out var result);

            // Assert
            success.ShouldBeTrue();
            result.ShouldBeFalse();
        }
    }
}
