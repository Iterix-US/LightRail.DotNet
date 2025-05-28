using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;
using System.Threading;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface INamedPipeConfigurator
    {
        /// <summary>
        /// Logger for logging named pipe operations and events.
        /// </summary>
        ILogger Logger { get; set; }

        /// <summary>
        /// Unique identifier for the named pipe configuration.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Indicates whether to use encryption for messages sent through the pipe.
        /// </summary>
        bool UseEncryption { get; set; }

        /// <summary>
        /// The name of the server that hosts the named pipe.
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// The name of the named pipe to be used for communication.
        /// </summary>
        string PipeName { get; }

        /// <summary>
        /// The encryption service used for securing messages.
        /// </summary>
        IEncryptionService EncryptionService { get; set; }

        /// <summary>
        /// Cancellation token source for managing cancellation of named pipe operations.
        /// </summary>
        CancellationTokenSource CancellationTokenSource { get; }
    }
}
