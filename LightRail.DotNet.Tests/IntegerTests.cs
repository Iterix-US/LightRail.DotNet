using LightRail.DotNet.Extensions;
using Shouldly;

namespace LightRail.DotNet.Tests
{
    public class IntegerTests
    {
        [Fact]
        public void IsBetween_ShouldReturnTrue_WhenValueIsBetweenMinAndMax()
        {
            // Arrange
            int value = 5;
            int minValue = 1;
            int maxValue = 10;

            // Act
            var result = value.IsBetween(minValue, maxValue);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void IsMultipleOf_ShouldReturnTrue_WhenValueIsMultipleOfGivenNumber()
        {
            // Arrange
            int value = 10;
            int multiple = 2;

            // Act
            var result = value.IsMultipleOf(multiple);

            // Assert
            result.ShouldBeTrue();
        }
    }
}
