using System;
using System.Threading.Tasks;
using SeroGlint.DotNet.NamedPipes.Delegates;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    /// <summary>
    /// Interface for a named pipe server that listens for messages and processes them asynchronously.
    /// </summary>
    public interface INamedPipeServer : IDisposable
    {
        /// <summary>
        /// Event that is triggered when a message is received on the named pipe.
        /// </summary>
        event PipeMessageReceivedHandler MessageReceived;

        /// <summary>
        /// Event that is triggered when a response is requested from the named pipe server.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();
    }
}
