using System;
using SeroGlint.DotNet.NamedPipes.Interfaces;

namespace SeroGlint.DotNet.NamedPipes.EventArguments
{
    public class PipeServerStateChangedEventArgs
    {
        public string ContextLabel { get; private set; }
        public string StateDescription { get; private set; }

        public static PipeServerStateChangedEventArgs SetPipeServerStopped(INamedPipeServer namedPipeServer)
        {
            return new PipeServerStateChangedEventArgs
            {
                ContextLabel = "Stopped",
                StateDescription = 
                    "Server is stopped and disposed. " +
                    $"Timestamp: {DateTime.Now}. " +
                    $"Configuration untouched. Server Id = {namedPipeServer.Id}"
            };
        }

        public static PipeServerStateChangedEventArgs SetPipeServerStarted(INamedPipeServer namedPipeServer)
        {
            return new PipeServerStateChangedEventArgs
            {
                ContextLabel = "Started",
                StateDescription = 
                    "Server is started and listening for connections. " +
                    $"Timestamp: {DateTime.Now}." +
                    $" Server Id = {namedPipeServer.Id}"
            };
        }

        public string GetLogMessage()
        {
            return $"{ContextLabel} | {StateDescription}";
        }
    }
}
