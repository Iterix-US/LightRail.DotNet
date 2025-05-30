using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces;

namespace SeroGlint.DotNet.NamedPipes
{
    [ExcludeFromCodeCoverage] // No need to test a wrapper pass-through
    public class PipeClientStreamWrapper : IPipeClientStreamWrapper
    {
        public Guid Id { get; } = Guid.NewGuid();
        private readonly NamedPipeClientStream _pipeClientStream;

        public PipeClientStreamWrapper(NamedPipeClientStream pipeClientStream = null)
        {
            _pipeClientStream = pipeClientStream;
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            await _pipeClientStream.ConnectAsync(token);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token)
        {
            await _pipeClientStream.WriteAsync(buffer, offset, count, token);
        }

        public async Task FlushAsync(CancellationToken token)
        {
            await _pipeClientStream.FlushAsync(token);
        }

        public void Dispose()
        {
            _pipeClientStream?.Dispose();
        }

        public async Task<int> ReadAsync(byte[] buffer, int i, int bufferLength, CancellationToken token)
        {
            return await _pipeClientStream.ReadAsync(buffer, i, bufferLength, token);
        }
    }
}
