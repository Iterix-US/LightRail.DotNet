using System;
using System.Collections.Generic;
using System.ComponentModel;
using LightRail.DotNet.Extensions;
using Shouldly;
using Xunit;

namespace LightRail.DotNet.Tests
{
    public class EnumExtensionsTests
    {
        private enum TestEnum
        {
            [Description("First Value")]
            First,

            [Description("Second Value")]
            Second,

            Third // No description
        }

        [Fact]
        public void GetDescription_ShouldReturnDescriptionAttribute_WhenPresent()
        {
            // Arrange
            var value = TestEnum.First;

            // Act
            var description = value.GetDescription();

            // Assert
            description.ShouldBe("First Value");
        }

        [Fact]
        public void GetDescription_ShouldFallbackToEnumName_WhenNoDescriptionAttribute()
        {
            // Arrange
            var value = TestEnum.Third;

            // Act
            var description = value.GetDescription();

            // Assert
            description.ShouldBe("Third");
        }

        [Fact]
        public void GetAllValues_ShouldReturnAllEnumValues()
        {
            // Arrange
            var dummy = TestEnum.First;

            // Act
            var values = dummy.GetAllValues();

            // Assert
            values.ShouldBe([TestEnum.First, TestEnum.Second, TestEnum.Third]);
        }
    }
}