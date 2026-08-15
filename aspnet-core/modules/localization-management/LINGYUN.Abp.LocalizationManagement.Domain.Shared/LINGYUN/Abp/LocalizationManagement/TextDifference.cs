namespace LINGYUN.Abp.LocalizationManagement;

public class TextDifference
{
    public int Id { get; set; }
    public string CultureName { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string? Value { get; set; }
    public string? ResourceName { get; set; }
    public string TargetCultureName { get; set; } = default!;
    public string? TargetValue { get; set; }

    public TextDifference() { }
    public TextDifference(
        int id,
        string cultureName,
        string key,
        string? value,
        string targetCultureName,
        string? targetValue = null,
        string? resourceName = null)
    {
        Id = id;
        Key = key;
        Value = value;
        CultureName = cultureName;
        TargetCultureName = targetCultureName;

        TargetValue = targetValue;
        ResourceName = resourceName;
    }
}
