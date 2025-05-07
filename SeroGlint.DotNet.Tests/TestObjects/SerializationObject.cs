using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace SeroGlint.DotNet.Tests.TestObjects
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(nameof(SerializationObject))]
    public class SerializationObject
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
