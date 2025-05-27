using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.NamedPipes
{
    public class PipeConfiguration
    {
        public IPipeMessageFormatter Formatter { get; set; }

        /// <summary>
        /// Indicates whether to use encryption for messages sent through the pipe.
        /// </summary>
        public bool UseEncryption { get; set; }

        /// <summary>
        /// The key used for securing messages.
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Defines the mode of the named pipe server (Perpetual or Momentary)
        /// </summary>
        public PipeServerMode ServerMode { get; set; } = PipeServerMode.Perpetual;
    }
}
