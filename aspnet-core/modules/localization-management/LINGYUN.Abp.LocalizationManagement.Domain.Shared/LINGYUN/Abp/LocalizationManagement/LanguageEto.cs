namespace LINGYUN.Abp.LocalizationManagement;
public class LanguageEto
{
    public bool Enable { get; set; }
    public string CultureName { get; set; } = default!;
    public string UiCultureName { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string TwoLetterISOLanguageName { get; set; } = default!;
}
