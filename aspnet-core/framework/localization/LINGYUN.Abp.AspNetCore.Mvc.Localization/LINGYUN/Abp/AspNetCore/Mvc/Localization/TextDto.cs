namespace LINGYUN.Abp.AspNetCore.Mvc.Localization;

public class TextDto
{
    public string Key { get; set; } = default!;
    public string? Value { get; set; }
    public string CultureName { get; set; } = default!;
    public string? ResourceName { get; set; }
}
