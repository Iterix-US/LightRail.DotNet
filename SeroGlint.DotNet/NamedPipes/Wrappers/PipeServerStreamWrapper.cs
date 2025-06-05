using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using SeroGlint.DotNet.NamedPipes.Interfaces;

namespace SeroGlint.DotNet.NamedPipes.Wrappers
{
    [ExcludeFromCodeCoverage] // No need to test a wrapper pass-through
    public class PipeServerStreamWrapper : IPipeServerStreamWrapper
    {
        public bool IsConnected => ServerStream.IsConnected;
        public NamedPipeServerStream ServerStream { get; }

        public PipeServerStreamWrapper(NamedPipeServerStream pipeServerStream = null)
        {
            ServerStream = pipeServerStream;
        }

        public async Task WaitForConnectionAsync(CancellationToken token)
        {
            await ServerStream.WaitForConnectionAsync(token);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token)
        {
            await ServerStream.WriteAsync(buffer, offset, count, token);
        }

        public async Task FlushAsync(CancellationToken token)
        {
            await ServerStream.FlushAsync(token);
        }

        public async Task<int> ReadAsync(byte[] buffer, int i, int bufferLength, CancellationToken token)
        {
            return await ServerStream.ReadAsync(buffer, i, bufferLength, token);
        }

        public void Dispose()
        {
            ServerStream?.Dispose();
        }
    }
}