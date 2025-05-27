using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;

namespace SeroGlint.DotNet.SecurityUtilities
{
    public class AesEncryptionService : IEncryptionService
    {
        private byte[] _key;
        private byte[] _initializationVector;
        private readonly ILogger _logger;

        public AesEncryptionService(string base64Key, ILogger logger)
        {
            _logger = logger;
            InitializeKey(base64Key);
        }

        private void InitializeKey(string base64Key)
        {
            _logger.LogInformation("Initializing encryption key");
            _key = Convert.FromBase64String(base64Key);

            if (_key.Length != 32)
            {
                _logger.LogError("Invalid AES key length: {Length} bytes", _key.Length);
                throw new ArgumentException("AES key must be 32 bytes (Base64-encoded).");
            }

            using (var sha256 = SHA256.Create())
            {
                _logger.LogInformation("Computing hash");
                var fullHash = sha256.ComputeHash(Encoding.UTF8.GetBytes("SeroGlintPipeIV"));
                _initializationVector = new byte[16];
                Array.Copy(fullHash, 0, _initializationVector, 0, 16);
                _logger.LogInformation("Initialized successfully");
            }
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
            {
                _logger.LogError("Cannot encrypt null or empty data.");
                throw new ArgumentException("Data to encrypt cannot be null or empty.");
            }

            _logger.LogInformation("Encrypting data using AES encryption");
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _initializationVector;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                _logger.LogInformation("Creating AES encryptor");
                using (var encryptor = aes.CreateEncryptor())
                {
                    _logger.LogInformation("Transforming plaintext into ciphertext");
                    return encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
                }
            }
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
            {
                _logger.LogError("Cannot decrypt null or empty data.");
                throw new ArgumentException("Data to decrypt cannot be null or empty.");
            }

            _logger.LogInformation("Decrypting data using AES decryption");
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _initializationVector;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                _logger.LogInformation("Creating AES decryptor");
                using (var decryptor = aes.CreateDecryptor())
                {
                    _logger.LogInformation("Transforming ciphertext into plaintext");
                    return decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                }
            }
        }
    }
}
