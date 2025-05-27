namespace SeroGlint.DotNet.NamedPipes.NamedPipeInterfaces
{
    public interface IPipeMessageFormatter
    {
        byte[] Serialize<T>(T message);
        T Deserialize<T>(byte[] data);
    }
}
