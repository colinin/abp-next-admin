namespace LINGYUN.Abp.LocalizationManagement;

public class TextDifferenceDto
{
    public string CultureName { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string? Value { get; set; }
    public string ResourceName { get; set; } = default!;
    public string TargetCultureName { get; set; } = default!;
    public string? TargetValue { get; set; }
}
