using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;
using SeroGlint.DotNet.SecurityUtilities.SecurityInterfaces;

namespace SeroGlint.DotNet.NamedPipes.Packaging
{
    /// <summary>
    /// Base class for configuring named pipe servers, providing default implementations and properties.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class NamedPipeConfigurationBase : INamedPipeConfigurator
    {
        public virtual ILogger Logger { get; set; }
        public virtual string Id { get; set; } = Guid.NewGuid().ToString("N");
        public virtual string ServerName { get; private set; } = $"{GetName()}-PipeServer";
        public virtual string PipeName { get; private set; } = $"{GetName()}-Pipe";
        public virtual bool UseEncryption { get; set; } = true;
        public virtual IEncryptionService EncryptionService { get; set; }
        public virtual CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

        private static string GetName() => AppDomain.CurrentDomain.FriendlyName.Split('.')[0];

        /// <summary>
        /// Sets the server name for the named pipe configuration.
        /// </summary>
        /// <param name="serverName"></param>
        public void SetServerName(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
            {
                Logger.LogWarning("Server name cannot be null or whitespace.");
            }

            ServerName = serverName;
        }

        /// <summary>
        /// Sets the pipe name for the named pipe configuration.
        /// </summary>
        /// <param name="pipeName"></param>
        public void SetPipeName(string pipeName)
        {
            if (!Debugger.IsAttached && string.IsNullOrWhiteSpace(pipeName))
            {
                Logger.LogWarning("Pipe name cannot be null or whitespace.");
            }

            PipeName = pipeName;
        }

        /// <summary>
        /// Injects an existing cancellation token source into the configuration.
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        public void InjectCancellationTokenSource(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
            {
                Logger.LogWarning("Cancellation token source cannot be null.");
                return;
            }

            CancellationTokenSource = cancellationTokenSource;
        }
    }
}
