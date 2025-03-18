namespace LightRail.DotNet.Tests.TestObjects
{
    public class InvalidObject
    {
        public Action NonSerializableProperty { get; set; } = () => Console.WriteLine("Test");
    }
}
