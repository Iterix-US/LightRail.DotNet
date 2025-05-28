using System.Threading;

namespace SeroGlint.DotNet.NamedPipes.Packaging
{
    public class PipeServerConfiguration : NamedPipeConfigurationBase
    {
        /// <summary>
        /// Unique identifier for the server configuration.
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Defines the mode of the named pipe server (Perpetual or Momentary)
        /// </summary>
        public PipeServerMode ServerMode { get; set; } = PipeServerMode.Perpetual;
    }
}
