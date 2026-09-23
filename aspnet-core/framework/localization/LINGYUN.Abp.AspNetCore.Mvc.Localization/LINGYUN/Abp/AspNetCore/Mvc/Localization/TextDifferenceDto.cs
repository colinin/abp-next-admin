namespace LINGYUN.Abp.AspNetCore.Mvc.Localization;

public class TextDifferenceDto
{
    public string CultureName { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string? Value { get; set; }
    public string ResourceName { get; set; } = default!;
    public string TargetCultureName { get; set; } = default!;
    public string? TargetValue { get; set; }

    public int CompareTo(TextDifferenceDto other)
    {
        return other.ResourceName.CompareTo(ResourceName) ^ other.Key.CompareTo(Key);
    }
}
