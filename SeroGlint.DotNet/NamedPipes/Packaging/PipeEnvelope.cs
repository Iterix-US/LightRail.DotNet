using System;
using System.Text;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.NamedPipes.Packaging
{
    public class PipeEnvelope<T> : IPipeEnvelope
    {
        private readonly ILogger _logger;

        public Guid MessageId { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string TypeName { get; set; } = typeof(T).FullName;
        public T Payload { get; set; }

        /// <summary>
        /// Serializes the current message to a byte array in JSON format.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public byte[] Serialize()
        {
            try
            {
                _logger.LogInformation("Serializing message to JSON and encrypting it.");

                var json = this.ToJson();
                var jsonBytes = Encoding.UTF8.GetBytes(json);

                _logger.LogInformation("Message serialized successfully.");
                return jsonBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to serialize message.");
                throw new InvalidOperationException("Failed to serialize message.", ex);
            }
        }

        public PipeEnvelope<TTargetObject> Deserialize<TTargetObject>(string json)
        {
            return Deserialize<TTargetObject>(json, _logger);
        }

        /// <summary>
        /// Deserializes a byte array into the specified target object type.
        /// </summary>
        /// <typeparam name="TTargetObject"></typeparam>
        /// <param name="json"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static PipeEnvelope<TTargetObject> Deserialize<TTargetObject>(string json, ILogger logger = null)
        {
            if (json.IsNullOrWhitespace() || !json.Trim().StartsWith("{"))
            {
                logger?.LogWarning("Content submitted for deserialization is not a valid json package");
            }

            try
            {
                var deserializedObject = json.FromJsonToType<PipeEnvelope<TTargetObject>>();
                logger?.LogInformation("Message deserialized successfully.");
                return deserializedObject;
            }
            catch (Exception ex)
            {
                const string errorMessage = "Failed to deserialize message.";
                logger?.LogError(ex, errorMessage);
                throw new InvalidOperationException(errorMessage, ex);
            }
        }
    }
}
