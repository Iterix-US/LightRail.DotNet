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
        public virtual string ServerName { get; private set; } = ".";
        public virtual string PipeName { get; private set; } = $"{GetName()}-Pipe";
        public virtual bool UseEncryption { get; set; } = true;
        public virtual IEncryptionService EncryptionService { get; private set; }
        public virtual CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

        private static string GetName() => AppDomain.CurrentDomain.FriendlyName.Split('.')[0];

        public void Initialize(
            string serverName, 
            string pipeName,
            ILogger logger,
            IEncryptionService encryptionService = null, 
            CancellationTokenSource cancellationTokenSource = null)
        {
            Logger = logger;
            
            if (string.IsNullOrWhiteSpace(serverName))
            {
                Logger.LogWarning("Server name cannot be null or whitespace.");
            }

            if (!Debugger.IsAttached && string.IsNullOrWhiteSpace(pipeName))
            {
                Logger.LogWarning("Pipe name cannot be null or whitespace.");
            }

            if (cancellationTokenSource == null)
            {
                Logger.LogWarning("Cancellation token source cannot be null.");
                return;
            }

            ServerName = serverName ?? ServerName;
            PipeName = pipeName ?? PipeName;
            EncryptionService = encryptionService ?? EncryptionService;
            CancellationTokenSource = cancellationTokenSource ?? CancellationTokenSource;
        }
    }
}
