using System.IO;
using System;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace LightRail.DotNet.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static string ToXml<T>(this T value, bool indent = false)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                var settings = new XmlWriterSettings
                {
                    Indent = indent,
                    OmitXmlDeclaration = false,
                    NewLineHandling = NewLineHandling.None
                };

                using (var stream = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stream, settings))
                    {
                        serializer.Serialize(writer, value);
                        return stream.ToString();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Error serializing object of type {typeof(T)} to XML.", ex);
            }
        }
    }
}
