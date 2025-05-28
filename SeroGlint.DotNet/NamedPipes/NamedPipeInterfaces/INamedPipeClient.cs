using System.Threading.Tasks;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface INamedPipeClient
    {
        Task SendMessage(string serverName, string pipeName, string messageContent, bool encryptMessage);
    }
}
