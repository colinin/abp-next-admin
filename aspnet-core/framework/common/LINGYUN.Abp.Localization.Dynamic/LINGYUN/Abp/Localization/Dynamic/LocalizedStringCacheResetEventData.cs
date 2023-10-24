namespace LINGYUN.Abp.Localization.Dynamic
{
    public class LocalizedStringCacheResetEventData
    {
        public bool IsDeleted { get; set; }
        public string ResourceName { get; set; }
        public string CultureName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public LocalizedStringCacheResetEventData()
        {

        }

        public LocalizedStringCacheResetEventData(
            string resourceName,
            string cultureName,
            string key,
            string value)
        {
            ResourceName = resourceName;
            CultureName = cultureName;
            Key = key;
            Value = value;

            IsDeleted = false;
        }
    }
}
