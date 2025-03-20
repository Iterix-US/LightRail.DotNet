using System.Diagnostics.CodeAnalysis;

namespace LightRail.DotNet.Tests.TestObjects
{
    [ExcludeFromCodeCoverage]
    public class InvalidObject
    {
        public Action NonSerializableProperty { get; set; } = () => Console.WriteLine("Test");
    }
}
