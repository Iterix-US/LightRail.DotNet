using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.NamedPipes;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.Tests.TestClasses.NamedPipes.TestObjects
{
    internal class TestableNamedPipeClient(
        INamedPipeConfigurator config,
        ILogger logger,
        IPipeClientStreamWrapper? wrapper = null)
        : NamedPipeClient(config, logger, wrapper)
    {
        public override async Task<string> Send<T>(IPipeEnvelope<T> message)
        {
            if (!ValidateMessageSettings(Configuration.ServerName, Configuration.PipeName, message))
            {
                return "Pipe envelop was empty. Refusing to send.";
            }

            return await Send(message);
        }
    }
}
