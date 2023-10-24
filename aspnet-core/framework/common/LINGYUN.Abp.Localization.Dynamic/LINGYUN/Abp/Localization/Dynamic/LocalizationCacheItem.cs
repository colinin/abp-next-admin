using System.Collections.Generic;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public class LocalizationCacheItem
    {
        public string Resource { get; set; }
        public string Culture { get; set; }
        public List<LocalizationText> Texts { get; set; }
        public LocalizationCacheItem()
        {
            Texts = new List<LocalizationText>();
        }

        public LocalizationCacheItem(
            string resource,
            string culture,
            List<LocalizationText> texts)
        {
            Resource = resource;
            Culture = culture;
            Texts = texts;
        }

        public static string NormalizeKey(
            string resource,
            string culture)
        {
            return $"p:Localization,r:{resource},c:{culture}";
        }
    }

    public class LocalizationText
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public LocalizationText()
        {

        }

        public LocalizationText(
            string key,
            string value)
        {
            Key = key;
            Value = value;
        }

    }
}
