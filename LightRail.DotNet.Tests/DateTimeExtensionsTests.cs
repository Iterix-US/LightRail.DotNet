using LightRail.DotNet.Extensions;
using Shouldly;

namespace LightRail.DotNet.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void Minus_WithYearsMonthsDays_ShouldReturnCorrectDateTime()
        {
            // Arrange
            var dateTime = new DateTime(2023, 10, 10);
            var expectedDateTime = new DateTime(2020, 7, 9);

            // Act
            var result = dateTime.Minus(years: 3, months: 3, days: 1);

            // Assert
            result.Date.ShouldBe(expectedDateTime.Date);
        }

        [Fact]
        public void Minus_WithDateTime_ShouldReturnCorrectDateTime()
        {
            // Arrange
            var dateTime = new DateTime(2023, 10, 10);
            var subtractionDateTime = new DateTime(2020, 8, 10);
            var expectedDateTime = DateTime.Parse("0003-1-31");

            // Act
            var result = dateTime.Minus(subtractionDateTime);

            // Assert
            result.ShouldBe(expectedDateTime);
        }

        [Fact]
        public void Plus_WithYearsMonthsDays_ShouldReturnCorrectDateTime()
        {
            // Arrange
            var dateTime = new DateTime(2020, 1, 1);
            var expectedDateTime = new DateTime(2023, 4, 1);

            // Act
            var result = dateTime.Plus(years: 3, months: 3, days: 0);

            // Assert
            result.ShouldBe(expectedDateTime);
        }

        [Fact]
        public void Plus_WithDateTime_ShouldReturnCorrectDateTime()
        {
            // Arrange
            var dateTime = new DateTime(2023, 10, 10);
            var additionDateTime = new DateTime(2020, 8, 10);
            var expectedDateTime = DateTime.Parse("4044-6-20");

            // Act
            var result = dateTime.Plus(additionDateTime);

            // Assert
            result.ShouldBe(expectedDateTime);
        }
    }
}
