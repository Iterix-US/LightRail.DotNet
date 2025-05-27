using System;
using System.Text;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.SecurityUtilities;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;

namespace SeroGlint.DotNet.NamedPipes
{
    public class EncryptedJsonMessageFormatter : IPipeMessageFormatter
    {
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger _logger;

        public EncryptedJsonMessageFormatter(string key, ILogger logger)
        {
            _logger = logger;
            _encryptionService = new AesEncryptionService(key, _logger);
        }

        public byte[] Serialize<T>(T message)
        {
            try
            {
                _logger.LogInformation("Serializing message to JSON and encrypting it.");
                var json = message.ToJson();
                var jsonBytes = Encoding.UTF8.GetBytes(json);
                var encryptedJson = _encryptionService.Encrypt(jsonBytes);
                _logger.LogInformation("Message serialized and encrypted successfully.");
                return encryptedJson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to serialize message.");
                throw new InvalidOperationException("Failed to serialize message.", ex);
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            try
            {
                _logger.LogInformation("Decrypting message and deserializing from JSON.");
                var decryptedBytes = _encryptionService.Decrypt(data);
                var decryptedJson = Encoding.UTF8.GetString(decryptedBytes);
                _logger.LogInformation("Message decrypted successfully.");
                return decryptedJson.FromJsonToType<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize message.");
                throw new InvalidOperationException("Failed to deserialize message.", ex);
            }
        }
    }
}
