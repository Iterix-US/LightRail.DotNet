using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightRail.DotNet.Extensions;
using Shouldly;

namespace LightRail.DotNet.Tests
{
    public class BooleanExtensionsTests
    {
        [Fact]
        public void ToInteger_ShouldReturnOneForTrue()
        {
            // Arrange
            bool boolean = true;

            // Act
            var result = boolean.ToInteger();

            // Assert
            result.ShouldBe(1);
        }

        [Fact]
        public void ToInteger_ShouldReturnZeroForFalse()
        {
            // Arrange
            bool boolean = false;

            // Act
            var result = boolean.ToInteger();

            // Assert
            result.ShouldBe(0);
        }

        [Fact]
        public void ToYesNo_ShouldReturnYesForTrue()
        {
            // Arrange
            bool boolean = true;

            // Act
            var result = boolean.ToYesNo();

            // Assert
            result.ShouldBe("Yes");
        }

        [Fact]
        public void ToYesNo_ShouldReturnNoForFalse()
        {
            // Arrange
            bool boolean = false;

            // Act
            var result = boolean.ToYesNo();

            // Assert
            result.ShouldBe("No");
        }

        [Fact]
        public void ToOneZero_ShouldReturnOneForTrue()
        {
            // Arrange
            bool boolean = true;

            // Act
            var result = boolean.ToOneZero();

            // Assert
            result.ShouldBe("1");
        }

        [Fact]
        public void ToOneZero_ShouldReturnZeroForFalse()
        {
            // Arrange
            bool boolean = false;

            // Act
            var result = boolean.ToOneZero();

            // Assert
            result.ShouldBe("0");
        }

        [Fact]
        public void ToTrueFalse_ShouldReturnTrueForTrue()
        {
            // Arrange
            bool boolean = true;

            // Act
            var result = boolean.ToTrueFalse();

            // Assert
            result.ShouldBe("True");
        }

        [Fact]
        public void ToTrueFalse_ShouldReturnFalseForFalse()
        {
            // Arrange
            bool boolean = false;

            // Act
            var result = boolean.ToTrueFalse();

            // Assert
            result.ShouldBe("False");
        }

        [Fact]
        public void ToOnOff_ShouldReturnOnForTrue()
        {
            // Arrange
            bool boolean = true;

            // Act
            var result = boolean.ToOnOff();

            // Assert
            result.ShouldBe("On");
        }

        [Fact]
        public void ToOnOff_ShouldReturnOffForFalse()
        {
            // Arrange
            bool boolean = false;

            // Act
            var result = boolean.ToOnOff();

            // Assert
            result.ShouldBe("Off");
        }

        [Fact]
        public void ToEnabledDisabled_ShouldReturnEnabledForTrue()
        {
            // Arrange
            bool boolean = true;

            // Act
            var result = boolean.ToEnabledDisabled();

            // Assert
            result.ShouldBe("Enabled");
        }

        [Fact]
        public void ToEnabledDisabled_ShouldReturnDisabledForFalse()
        {
            // Arrange
            bool boolean = false;

            // Act
            var result = boolean.ToEnabledDisabled();

            // Assert
            result.ShouldBe("Disabled");
        }

        [Fact]
        public void ToActiveInactive_ShouldReturnActiveForTrue()
        {
            // Arrange
            bool boolean = true;

            // Act
            var result = boolean.ToActiveInactive();

            // Assert
            result.ShouldBe("Active");
        }

        [Fact]
        public void ToActiveInactive_ShouldReturnInactiveForFalse()
        {
            // Arrange
            bool boolean = false;

            // Act
            var result = boolean.ToActiveInactive();

            // Assert
            result.ShouldBe("Inactive");
        }
    }
}
