using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Xml
{
    public static class XmlLocalizationDictionaryBuilder
    {
        public static ILocalizationDictionary BuildFromFile(string filePath)
        {
            try
            {
                return BuildFromXmlString(File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                throw new AbpException("Invalid localization file format: " + filePath, ex);
            }
        }

        public static ILocalizationDictionary BuildFromXmlString(string xmlString)
        {
            XmlLocalizationFile xmlFile;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XmlLocalizationFile));
                using (StringReader reader = new StringReader(xmlString))
                {
                    xmlFile = (XmlLocalizationFile)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new AbpException("Can not parse xml string. " + ex.Message);
            }

            var cultureCode = xmlFile.Culture.Name;
            if (string.IsNullOrEmpty(cultureCode))
            {
                throw new AbpException("Culture is empty in language json file.");
            }

            var dictionary = new Dictionary<string, LocalizedString>();
            var dublicateNames = new List<string>();
            foreach (var item in xmlFile.Texts)
            {
                if (string.IsNullOrEmpty(item.Key))
                {
                    throw new AbpException("The key is empty in given json string.");
                }

                if (dictionary.GetOrDefault(item.Key) != null)
                {
                    dublicateNames.Add(item.Key);
                }

                dictionary[item.Key] = new LocalizedString(item.Key, item.Value.NormalizeLineEndings());
            }

            if (dublicateNames.Count > 0)
            {
                throw new AbpException(
                    "A dictionary can not contain same key twice. There are some duplicated names: " +
                    dublicateNames.JoinAsString(", "));
            }

            return new StaticLocalizationDictionary(cultureCode, dictionary);
        }
    }
}
