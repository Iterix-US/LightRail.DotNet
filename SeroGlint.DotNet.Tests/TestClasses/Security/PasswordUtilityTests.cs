using System.Text;
using SeroGlint.DotNet.Security;
using Shouldly;

namespace SeroGlint.DotNet.Tests.TestClasses.Security
{
    public class PasswordUtilityTests
    {
        [Fact]
        public void PasswordUtility_WhenHashingSamePasswordMultipleTimes_ThenHashesShouldDiffer()
        {
            // Arrange
            var password = "MySecurePassword";

            // Act
            var hash1 = PasswordUtility.HashPassword(password);
            var hash2 = PasswordUtility.HashPassword(password);

            // Assert
            hash1.ShouldNotBe(hash2);
        }

        [Fact]
        public void PasswordUtility_WhenVerifyingCorrectPassword_ThenReturnsTrue()
        {
            var password = "CorrectHorseBatteryStaple";
            var hash = PasswordUtility.HashPassword(password);

            var result = PasswordUtility.VerifyPassword(password, hash);

            result.ShouldBeTrue();
        }

        [Fact]
        public void PasswordUtility_WhenVerifyingIncorrectPassword_ThenReturnsFalse()
        {
            var correct = "Secure123!";
            var wrong = "Insecure123!";
            var hash = PasswordUtility.HashPassword(correct);

            var result = PasswordUtility.VerifyPassword(wrong, hash);

            result.ShouldBeFalse();
        }

        [Fact]
        public void PasswordUtility_WhenVerifyingTamperedHash_ThenReturnsFalse()
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
        public void PasswordUtility_WhenVerifyingMalformedHash_ThenReturnsFalse()
        {
            var malformed = "PBKDF2-SHA256:100000:this-is-not-base64";

            var result = PasswordUtility.VerifyPassword("anything", malformed);

            result.ShouldBeFalse();
        }

        [Fact]
        public void PasswordUtility_WhenHashUsesUnsupportedAlgorithm_ThenReturnsFalse()
        {
            var hash = PasswordUtility.HashPassword("mypassword");
            var parts = hash.Split(':');
            parts[0] = "PBKDF2-SHA1";
            var modified = string.Join(":", parts);

            PasswordUtility.VerifyPassword("mypassword", modified).ShouldBeFalse();
        }

        [Fact]
        public void PasswordUtility_WhenHashingUnicodeEquivalents_ThenVerificationSucceeds()
        {
            var composed = "ﬁ";
            var decomposed = "f\u0069";

            var hash = PasswordUtility.HashPassword(composed);
            var result = PasswordUtility.VerifyPassword(decomposed, hash);

            result.ShouldBeTrue();
        }

        [Fact]
        public void PasswordUtility_WhenFixedTimeEqualsReceivesMatchingArrays_ThenReturnsTrue()
        {
            var a = new byte[] { 0xAA, 0xBB, 0xCC };
            var b = new byte[] { 0xAA, 0xBB, 0xCC };

            PasswordUtility.FixedTimeEquals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void PasswordUtility_WhenFixedTimeEqualsReceivesMismatchedArrays_ThenReturnsFalse()
        {
            var a = new byte[] { 0xAA, 0xBB, 0xCC };
            var b = new byte[] { 0xAA, 0xBB, 0xCD };

            PasswordUtility.FixedTimeEquals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void PasswordUtility_WhenFixedTimeEqualsReceivesArraysOfDifferentLengths_ThenReturnsFalse()
        {
            var a = new byte[] { 0xAA, 0xBB };
            var b = new byte[] { 0xAA, 0xBB, 0xCC };

            PasswordUtility.FixedTimeEquals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void PasswordUtility_WhenCallingPbkdf2HmacSha256_ThenReturnsExpectedByteLength()
        {
            var password = "secret";
            var salt = new byte[16];
            new Random(123).NextBytes(salt);

            var hash = PasswordUtility.Pbkdf2HmacSha256(password, salt, 1000, 64);

            hash.Length.ShouldBe(64);
        }

        [Fact]
        public void PasswordUtility_WhenCallingPbkdf2HmacSha256WithSameInputs_ThenReturnsDeterministicOutput()
        {
            var password = "predictable";
            var salt = Encoding.UTF8.GetBytes("this-is-salt");

            var h1 = PasswordUtility.Pbkdf2HmacSha256(password, salt, 5000, 32);
            var h2 = PasswordUtility.Pbkdf2HmacSha256(password, salt, 5000, 32);

            h1.ShouldBe(h2);
        }

        [Fact]
        public void PasswordUtility_WhenSaltIsInvalidBase64_ThenVerifyReturnsFalse()
        {
            // Arrange
            var invalidSalt = "!!!!";
            var hash = $"PBKDF2-SHA256:100000:{invalidSalt}:aGVsbG93b3JsZA==";

            // Act
            var result = PasswordUtility.VerifyPassword("test", hash);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void PasswordUtility_WhenHashIsInvalidBase64_ThenVerifyReturnsFalse()
        {
            // Arrange
            var salt = Convert.ToBase64String("salt123"u8.ToArray());
            var invalidHash = "%%%";
            var hash = $"PBKDF2-SHA256:100000:{salt}:{invalidHash}";

            // Act
            var result = PasswordUtility.VerifyPassword("test", hash);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
