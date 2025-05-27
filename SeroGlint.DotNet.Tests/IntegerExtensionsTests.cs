using SeroGlint.DotNet.Extensions;
using Shouldly;

namespace SeroGlint.DotNet.Tests
{
    public class IntegerExtensionsTests
    {
        [Fact]
        public void IsBetween_ShouldReturnTrue_WhenValueIsBetweenMinAndMax()
        {
            // Arrange
            const int value = 5;
            const int minValue = 1;
            const int maxValue = 10;

            // Act
            var result = value.IsBetween(minValue, maxValue);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsMultipleOf_ShouldReturnTrue_WhenValueIsMultipleOfGivenNumber()
        {
            // Arrange
            const int value = 10;
            const int multiple = 2;

            // Act
            var result = value.IsMultipleOf(multiple);

            // Assert
            result.ShouldBeTrue();
        }
    }
}
