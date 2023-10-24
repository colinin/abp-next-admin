using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LINGYUN.Abp.Localization.Xml
{
    [Serializable]
    [XmlRoot(ElementName = "localization")]
    public class XmlLocalizationFile
    {
        [XmlElement("culture")]
        public CultureInfo Culture { get; set; } = new CultureInfo();

        [XmlArray("texts")]
        [XmlArrayItem("text")]
        public LocalizationText[] Texts { get; set; } = new LocalizationText[0];

        public XmlLocalizationFile() { }

        public XmlLocalizationFile(string culture)
        {
            Culture = new CultureInfo(culture);
        }

        public XmlLocalizationFile(string culture, LocalizationText[] texts)
        {
            Culture = new CultureInfo(culture);
            Texts = texts;
        }

        public void WriteToPath(string filePath)
        {
            var fileName = Path.Combine(filePath, Culture.Name + ".xml");
            using FileStream fileStream = new(fileName, FileMode.Create, FileAccess.Write);
            InternalWrite(fileStream, Encoding.UTF8);
        }

        private void InternalWrite(Stream stream, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            XmlSerializer serializer = new(GetType());
            XmlWriterSettings settings = new()
            {
                Indent = true,
                NewLineChars = "\r\n",
                Encoding = encoding,
                IndentChars = " "
            };

            using XmlWriter writer = XmlWriter.Create(stream, settings);
            serializer.Serialize(writer, this);
            writer.Close();
        }
    }

    [Serializable]
    public class CultureInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public CultureInfo()
        {
        }

        public CultureInfo(string name)
        {
            Name = name;
        }
    }

    [Serializable]
    public class LocalizationText
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public LocalizationText()
        {

        }

        public LocalizationText(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
