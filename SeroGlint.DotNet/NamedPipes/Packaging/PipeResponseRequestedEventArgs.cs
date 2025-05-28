using System;
using System.IO.Pipes;

namespace SeroGlint.DotNet.NamedPipes.Packaging
{
    public class PipeResponseRequestedEventArgs : EventArgs
    {
        public Guid CorrelationId { get; }
        public object ResponseObject { get; }
        public NamedPipeServerStream Stream { get; }

        public PipeResponseRequestedEventArgs(Guid correlationId, object responseObject, NamedPipeServerStream stream)
        {
            CorrelationId = correlationId;
            ResponseObject = responseObject;
            Stream = stream;
        }
    }
}
