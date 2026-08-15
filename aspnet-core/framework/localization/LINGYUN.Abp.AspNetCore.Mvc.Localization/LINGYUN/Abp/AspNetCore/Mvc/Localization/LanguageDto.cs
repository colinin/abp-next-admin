namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    public class LanguageDto
    {
        public string CultureName { get; set; } = default!;
        public string UiCultureName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string TwoLetterISOLanguageName { get; set; } = default!;
    }
}
