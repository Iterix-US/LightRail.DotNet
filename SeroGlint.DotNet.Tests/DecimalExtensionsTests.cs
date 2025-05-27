using SeroGlint.DotNet.Extensions;
using Shouldly;

namespace SeroGlint.DotNet.Tests
{
    public class DecimalExtensionsTests
    {
        [Fact]
        public void IsPositive_ShouldReturnTrue_WhenValueIsPositive()
        {
            // Arrange
            const decimal value = 10.5m;

            // Act
            var result = value.IsPositive();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsNegative_ShouldReturnTrue_WhenValueIsNegative()
        {
            // Arrange
            const decimal value = -10.5m;

            // Act
            var result = value.IsNegative();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsZero_ShouldReturnTrue_WhenValueIsZero()
        {
            // Arrange
            const decimal value = 0m;

            // Act
            var result = value.IsZero();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsBetween_ShouldReturnTrue_WhenValueIsBetweenMinAndMax()
        {
            // Arrange
            const decimal value = 5m;
            const decimal minValue = 1m;
            const decimal maxValue = 10m;

            // Act
            var result = value.IsBetween(minValue, maxValue);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsMultipleOf_ShouldReturnTrue_WhenValueIsMultipleOfGivenNumber()
        {
            // Arrange
            const decimal value = 10m;
            const decimal multiple = 2m;

            // Act
            var result = value.IsMultipleOf(multiple);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsEven_ShouldReturnTrue_WhenValueIsEven()
        {
            // Arrange
            const decimal value = 4m;

            // Act
            var result = value.IsEven();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsOdd_ShouldReturnTrue_WhenValueIsOdd()
        {
            // Arrange
            const decimal value = 3m;

            // Act
            var result = value.IsOdd();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsInteger_ShouldReturnTrue_WhenValueIsInteger()
        {
            // Arrange
            const decimal value = 5m;

            // Act
            var result = value.IsInteger();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void ToIntegerCeiling_ShouldReturnCeilingValue()
        {
            // Arrange
            const decimal value = 5.3m;

            // Act
            var result = value.ToIntegerCeiling();

            // Assert
            result.ShouldBe(6);
        }

        [Fact]
        public void ToIntegerFloor_ShouldReturnFloorValue()
        {
            // Arrange
            const decimal value = 5.7m;

            // Act
            var result = value.ToIntegerFloor();

            // Assert
            result.ShouldBe(5);
        }

        [Fact]
        public void ToIntegerRound_ShouldReturnRoundedValue()
        {
            // Arrange
            const decimal value = 5.5m;

            // Act
            var result = value.ToIntegerRound();

            // Assert
            result.ShouldBe(6);
        }

        [Fact]
        public void ToIntegerTruncate_ShouldReturnTruncatedValue()
        {
            // Arrange
            const decimal value = 5.9m;

            // Act
            var result = value.ToIntegerTruncate();

            // Assert
            result.ShouldBe(5);
        }

        [Fact]
        public void ToDecimalString_ShouldReturnStringWithSpecifiedDecimalPoints()
        {
            // Arrange
            const decimal value = 5.12345m;
            const int decimalPoints = 3;

            // Act
            var result = value.ToDecimalString(decimalPoints);

            // Assert
            result.ShouldBe("5.123");
        }
    }
}
