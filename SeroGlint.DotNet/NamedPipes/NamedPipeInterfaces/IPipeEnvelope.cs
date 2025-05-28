using SeroGlint.DotNet.NamedPipes.Packaging;

namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface IPipeEnvelope
    {
        byte[] Serialize();
        PipeEnvelope<TTargetObject> Deserialize<TTargetObject>(string json);
    }
}
