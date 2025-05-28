using System;

namespace SeroGlint.DotNet.NamedPipes.Packaging
{
    public class PipeMessageReceivedEventArgs : EventArgs
    {
        public string Json { get; }
        public object DeserializedMessage { get; }

        public PipeMessageReceivedEventArgs(string json, object deserializedMessage)
        {
            Json = json;
            DeserializedMessage = deserializedMessage;
        }
    }
}
