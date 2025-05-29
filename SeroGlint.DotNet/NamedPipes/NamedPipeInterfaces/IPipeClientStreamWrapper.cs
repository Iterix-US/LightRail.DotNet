using System;
using System.Threading;
using System.Threading.Tasks;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface IPipeClientStreamWrapper : IDisposable
    {
        Guid Id { get; }
        Task ConnectAsync(CancellationToken token);
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token);
        Task FlushAsync(CancellationToken token);
    }
}
