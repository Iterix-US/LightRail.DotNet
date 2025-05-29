using System.IO.Pipes;

namespace SeroGlint.DotNet.Tests.Utilities
{
    internal static class TestNamedPipeServer
    {
        internal static async Task StartTestServerAsync(string pipeName, CancellationToken token, TaskCompletionSource? serverReady = null)
        {
            await using var server = new NamedPipeServerStream(
                pipeName,
                PipeDirection.InOut,
                1,
                transmissionMode: PipeTransmissionMode.Message,
                PipeOptions.Asynchronous);

            serverReady?.SetResult(); // ✅ Notify the test that the server is ready

            await server.WaitForConnectionAsync(token);

            var buffer = new byte[1024];
            _ = await server.ReadAsync(buffer, 0, buffer.Length, token);
        }
    }
}
