using System;
using System.Collections.Generic;
using System.Text;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    /// <summary>
    /// Interface for a named pipe server decorator that provides access to the core functionality of the named pipe server.
    /// </summary>
    public interface INamedPipeServerDecorator : INamedPipeServer
    {
        /// <summary>
        /// Core functionality of the named pipe server, providing access to the underlying server operations.
        /// </summary>
        INamedPipeServerCore Core { get; }
    }
}
