using System.Text;
using SeroGlint.DotNet.SecurityUtilities;
using Shouldly;

namespace SeroGlint.DotNet.Tests.TestClasses.Security
{
    public class PasswordUtilityTests
    {
        [Fact]
        public void HashPassword_ShouldReturnDifferentHashes_ForSamePassword()
        {
            // Arrange
            var password = "MySecurePassword";

            // Act
            var hash1 = PasswordUtility.HashPassword(password);
            var hash2 = PasswordUtility.HashPassword(password);

            // Assert
            hash1.ShouldNotBe(hash2); // Different salt should make them unique
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            var password = "CorrectHorseBatteryStaple";
            var hash = PasswordUtility.HashPassword(password);

            var result = PasswordUtility.VerifyPassword(password, hash);

            result.ShouldBeTrue();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            var correct = "Secure123!";
            var wrong = "Insecure123!";
            var hash = PasswordUtility.HashPassword(correct);

            var result = PasswordUtility.VerifyPassword(wrong, hash);

            result.ShouldBeFalse();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForTamperedHash()
        {
            var password = "DoNotTamper";
            var hash = PasswordUtility.HashPassword(password);

            var parts = hash.Split(':');
            var tamperedHashBytes = Convert.FromBase64String(parts[3]);
            tamperedHashBytes[0] ^= 0xFF;
            parts[3] = Convert.ToBase64String(tamperedHashBytes);
            var tampered = string.Join(":", parts);

            PasswordUtility.VerifyPassword(password, tampered).ShouldBeFalse();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForMalformedHash()
        {
            var malformed = "PBKDF2-SHA256:100000:this-is-not-base64";

            var result = PasswordUtility.VerifyPassword("anything", malformed);

            result.ShouldBeFalse();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForWrongAlgorithm()
        {
            var hash = PasswordUtility.HashPassword("mypassword");
            var parts = hash.Split(':');
            parts[0] = "PBKDF2-SHA1";
            var modified = string.Join(":", parts);

            PasswordUtility.VerifyPassword("mypassword", modified).ShouldBeFalse();
        }

        [Fact]
        public void HashPassword_ShouldNormalizeUnicode()
        {
            var composed = "ﬁ";             // U+FB01 (ligature)
            var decomposed = "f\u0069";     // 'f' + 'i'

            var hash = PasswordUtility.HashPassword(composed);
            var result = PasswordUtility.VerifyPassword(decomposed, hash);

            result.ShouldBeTrue(); // Because both normalize to same string
        }

        [Fact]
        public void FixedTimeEquals_ShouldReturnTrue_WhenArraysMatch()
        {
            var a = new byte[] { 0xAA, 0xBB, 0xCC };
            var b = new byte[] { 0xAA, 0xBB, 0xCC };

            PasswordUtility.FixedTimeEquals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void FixedTimeEquals_ShouldReturnFalse_WhenArraysAreDifferent()
        {
            var a = new byte[] { 0xAA, 0xBB, 0xCC };
            var b = new byte[] { 0xAA, 0xBB, 0xCD };

            PasswordUtility.FixedTimeEquals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void FixedTimeEquals_ShouldReturnFalse_WhenLengthsDiffer()
        {
            var a = new byte[] { 0xAA, 0xBB };
            var b = new byte[] { 0xAA, 0xBB, 0xCC };

            PasswordUtility.FixedTimeEquals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void Pbkdf2HmacSha256_ShouldReturnCorrectLength()
        {
            var password = "secret";
            var salt = new byte[16];
            new Random(123).NextBytes(salt);

            var hash = PasswordUtility.Pbkdf2HmacSha256(password, salt, 1000, 64);

            hash.Length.ShouldBe(64);
        }

        [Fact]
        public void Pbkdf2HmacSha256_ShouldBeDeterministic_ForSameInputs()
        {
            var password = "predictable";
            var salt = Encoding.UTF8.GetBytes("this-is-salt");

            var h1 = PasswordUtility.Pbkdf2HmacSha256(password, salt, 5000, 32);
            var h2 = PasswordUtility.Pbkdf2HmacSha256(password, salt, 5000, 32);

            h1.ShouldBe(h2);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenSaltIsInvalidBase64()
        {
            // Arrange
            var invalidSalt = "!!!!"; // invalid base64
            var hash = $"PBKDF2-SHA256:100000:{invalidSalt}:aGVsbG93b3JsZA==";

            // Act
            var result = PasswordUtility.VerifyPassword("test", hash);

            // Assert
            result.ShouldBeFalse(); // Should hit the catch block
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenHashIsInvalidBase64()
        {
            // Arrange
            var salt = Convert.ToBase64String(Encoding.UTF8.GetBytes("salt123"));
            var invalidHash = "%%%"; // not valid base64
            var hash = $"PBKDF2-SHA256:100000:{salt}:{invalidHash}";

            // Act
            var result = PasswordUtility.VerifyPassword("test", hash);

            // Assert
            result.ShouldBeFalse(); // Will trigger exception and hit catch
        }
    }
}
