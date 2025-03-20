﻿using System.IO;
using System;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace LightRail.DotNet.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Serializes an object to JSON.
        /// </summary>
        /// <param name="obj">The object to serialize into JSON</param>
        /// <returns>The JSON rendered through serialization</returns>
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// Serializes an object to an XML string
        /// </summary>
        /// <typeparam name="T">The type of the object to cast convert into the XML string</typeparam>
        /// <param name="obj">The object to convert into XML</param>
        /// <param name="indent">True: indents/formats the XML. False: leaves string as a single line.</param>
        /// <returns>The object as an XML string.</returns>
        /// <exception cref="InvalidOperationException">The object could not be serialized.</exception>
        public static string ToXml<T>(this T obj, bool indent = false)
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
                        serializer.Serialize(writer, obj);
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
