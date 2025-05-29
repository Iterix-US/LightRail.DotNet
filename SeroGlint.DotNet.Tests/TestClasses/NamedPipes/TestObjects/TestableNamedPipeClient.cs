using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.NamedPipes;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.Tests.TestClasses.NamedPipes.TestObjects
{
    internal class TestableNamedPipeClient(
        INamedPipeConfigurator config,
        ILogger logger,
        IPipeClientStreamWrapper? wrapper = null)
        : NamedPipeClient(config, logger)
    {
        public override async Task SendMessage<T>(IPipeEnvelope<T> message)
        {
            if (!ValidateMessageSettings(Configuration.ServerName, Configuration.PipeName, message))
            {
                return;
            }

            var serializedEnvelope = message.Serialize();
            await SendWithTimeoutInternal(wrapper, serializedEnvelope, Configuration.CancellationTokenSource.Token);
        }
    }
}
