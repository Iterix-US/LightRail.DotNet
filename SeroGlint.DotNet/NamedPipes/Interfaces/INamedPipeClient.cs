﻿using System.Threading.Tasks;

namespace SeroGlint.DotNet.NamedPipes.Interfaces
{
    /// <summary>
    /// Interface for a named pipe client that sends messages to a named pipe server.
    /// </summary>
    public interface INamedPipeClient
    {
        /// <summary>
        /// Sends a message to the named pipe server using the specified pipe envelope.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pipeEnvelope"></param>
        /// <returns></returns>
        Task<string> SendAsync<T>(IPipeEnvelope<T> pipeEnvelope);

        void Disconnect();
    }
}
