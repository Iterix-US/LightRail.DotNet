﻿using System.Diagnostics.CodeAnalysis;

namespace SeroGlint.DotNet.NamedPipes.Objects
{
    /// <summary>
    /// Configuration class for a named pipe server.
    /// </summary>
    [ExcludeFromCodeCoverage]
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
