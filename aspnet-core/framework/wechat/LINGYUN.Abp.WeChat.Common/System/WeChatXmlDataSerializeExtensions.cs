using LINGYUN.Abp.WeChat.Common.Messages;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace System;
public static class WeChatXmlDataSerializeExtensions
{
    private readonly static Hashtable _xmlSerializers = new();
    private readonly static XmlRootAttribute _xmlRoot = new("xml");

    private static XmlSerializer GetTypedSerializer(Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var skey = type.AssemblyQualifiedName ?? type.GetHashCode().ToString();
        if (_xmlSerializers[skey] is not XmlSerializer xmlSerializer)
        {
            xmlSerializer = new XmlSerializer(type, _xmlRoot);
            _xmlSerializers[skey] = xmlSerializer;
        }

        return xmlSerializer;
    }

    public static T DeserializeWeChatMessage<T>(this string xml, XmlDeserializationEvents? events = null) where T : WeChatMessage
    {
        var objectType = typeof(T);
        using var stringReader = new StringReader(xml);
        using var xmlReader = XmlReader.Create(stringReader);
        var serializer = GetTypedSerializer(objectType);
        var usingEvents = events ?? new XmlDeserializationEvents();
        return (T)serializer.Deserialize(xmlReader, usingEvents);
    }

    public static string SerializeWeChatMessage(this WeChatMessage message)
    {
        var objectType = message.GetType();
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = false,
            OmitXmlDeclaration = true,
            WriteEndDocumentOnClose = false,
            NamespaceHandling = NamespaceHandling.OmitDuplicates
        };
        using var stream = new MemoryStream();
        using var writer = XmlWriter.Create(stream, settings);
        var serializer = GetTypedSerializer(objectType);
        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, string.Empty);
        serializer.Serialize(writer, message, ns);
        writer.Flush();
        var xml = Encoding.UTF8.GetString(stream.ToArray());
        xml = Regex.Replace(xml, "\\s*<\\w+ ([a-zA-Z0-9]+):nil=\"true\"[^>]*/>", string.Empty, RegexOptions.IgnoreCase);
        xml = Regex.Replace(xml, "<\\?xml[^>]*\\?>", string.Empty, RegexOptions.IgnoreCase);

        return xml;
    }
}
