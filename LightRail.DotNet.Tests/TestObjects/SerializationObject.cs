using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace LightRail.DotNet.Tests.TestObjects
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(nameof(SerializationObject))]
    public class SerializationObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
