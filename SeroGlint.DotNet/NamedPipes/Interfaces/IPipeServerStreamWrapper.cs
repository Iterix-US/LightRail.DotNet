using System.Threading.Tasks;
using System.Threading;
using System.IO.Pipes;

namespace SeroGlint.DotNet.NamedPipes.Interfaces
{
    public interface IPipeServerStreamWrapper
    {
        NamedPipeServerStream ServerStream { get; }
        bool IsConnected { get; }
        Task WaitForConnectionAsync(CancellationToken token);
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token);
        Task FlushAsync(CancellationToken token);
        Task<int> ReadAsync(byte[] buffer, int i, int bufferLength, CancellationToken token);
    }
}
