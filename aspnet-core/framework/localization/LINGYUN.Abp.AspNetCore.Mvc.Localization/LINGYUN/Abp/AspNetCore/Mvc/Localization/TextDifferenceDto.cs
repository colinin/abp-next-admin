namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    public class TextDifferenceDto
    {
        public string CultureName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string ResourceName { get; set; }
        public string TargetCultureName { get; set; }
        public string TargetValue { get; set; }

        public int CompareTo(TextDifferenceDto other)
        {
            return other.ResourceName.CompareTo(ResourceName) ^ other.Key.CompareTo(Key);
        }
    }
}
