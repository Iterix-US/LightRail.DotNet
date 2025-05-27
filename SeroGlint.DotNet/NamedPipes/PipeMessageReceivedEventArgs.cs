using System;

namespace SeroGlint.DotNet.NamedPipes
{
    public class PipeMessageReceivedEventArgs : EventArgs
    {
        public string RawJson { get; }
        public object DeserializedMessage { get; }

        public PipeMessageReceivedEventArgs(string rawJson, object deserializedMessage)
        {
            RawJson = rawJson;
            DeserializedMessage = deserializedMessage;
        }
    }
}
