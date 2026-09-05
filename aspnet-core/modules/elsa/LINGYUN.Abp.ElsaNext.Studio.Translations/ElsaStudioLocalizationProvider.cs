using Elsa.Studio.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Microsoft.Extensions.Localization;

namespace LINGYUN.Abp.ElsaNext.Studio.Translations;

public class ElsaStudioLocalizationProvider : ILocalizationProvider
{
    private readonly IStringLocalizer<ElsaStudioResource> _localizer;


    public ElsaStudioLocalizationProvider(
        IStringLocalizer<ElsaStudioResource> localizer)
    {
        _localizer = localizer;
    }

    public string? GetTranslation(string key)
    {
        var localized = _localizer[key];
        return localized.ResourceNotFound ? null : localized.Value;
    }
}
