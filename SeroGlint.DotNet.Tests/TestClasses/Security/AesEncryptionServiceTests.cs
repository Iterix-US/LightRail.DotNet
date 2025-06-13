using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SeroGlint.DotNet.SecurityUtilities;

namespace SeroGlint.DotNet.Tests.TestClasses.Security
{
    public class AesEncryptionServiceTests
    {
        [Fact]
        public void AesEncryptionService_WhenEncryptingAndDecrypting_ThenReturnsOriginalPlaintext()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var key = Convert.ToBase64String("12345678901234567890123456789012"u8.ToArray()); // 32-byte key
            var service = new AesEncryptionService(key, logger);
            var originalText = "Hello, SeroGlint!";
            var originalBytes = Encoding.UTF8.GetBytes(originalText);

            // Act
            var encrypted = service.Encrypt(originalBytes);
            var decrypted = service.Decrypt(encrypted);
            var decryptedText = Encoding.UTF8.GetString(decrypted);

            // Assert
            Assert.Equal(originalText, decryptedText);
        }

        [Fact]
        public void AesEncryptionService_WhenInitializedWithInvalidKeyLength_ThenThrowsArgumentException()
        {
            var logger = Substitute.For<ILogger>();
            var invalidKey = Convert.ToBase64String("TooShortKey"u8.ToArray());

            Assert.Throws<ArgumentException>(() => new AesEncryptionService(invalidKey, logger));
        }

        [Fact]
        public void AesEncryptionService_WhenEncryptingNullData_ThenThrowsArgumentException()
        {
            var logger = Substitute.For<ILogger>();
            var key = Convert.ToBase64String("12345678901234567890123456789012"u8.ToArray()); // 32-byte key
            var service = new AesEncryptionService(key, logger);

            Assert.Throws<ArgumentException>(() => service.Encrypt(null));
        }

        [Fact]
        public void AesEncryptionService_WhenDecryptingEmptyArray_ThenThrowsArgumentException()
        {
            var logger = Substitute.For<ILogger>();
            var key = Convert.ToBase64String("12345678901234567890123456789012"u8.ToArray()); // 32-byte key
            var service = new AesEncryptionService(key, logger);

            Assert.Throws<ArgumentException>(() => service.Decrypt(Array.Empty<byte>()));
        }

        [Fact]
        public void AesEncryptionService_WhenGeneratingKey_ThenReturnsValidBase64Encoded32ByteKey()
        {
            // Act
            var key = AesEncryptionService.GenerateKey();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(key));

            byte[] keyBytes = Convert.FromBase64String(key);
            Assert.Equal(32, keyBytes.Length);
        }
    }
}
