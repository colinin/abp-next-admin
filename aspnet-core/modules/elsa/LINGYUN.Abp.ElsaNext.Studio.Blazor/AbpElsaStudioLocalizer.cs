using Elsa.Studio.Localization;
using LINGYUN.Abp.ElsaNext.Localization;
using Microsoft.Extensions.Localization;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor;

public class AbpElsaStudioLocalizer(
    IStringLocalizer<ElsaNextResource> _stringLocalizer,
    ILocalizationProvider _provider) : ILocalizer
{
    public LocalizedString this[string? key] {
        get {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new LocalizedString(string.Empty, string.Empty, true);
            }

            string? translation;
            var notFound = false;
            var localized = _stringLocalizer[key];
            if (localized.ResourceNotFound)
            {
                translation = _provider.GetTranslation(key);
                if (string.IsNullOrEmpty(translation))
                {
                    translation = key;
                    notFound = true;
                }
            }
            else
            {
                translation = localized.Value;
            }
            return new LocalizedString(key, translation, notFound);
        }
    }

    public LocalizedString this[string? key, params object[] arguments] {
        get {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new LocalizedString(string.Empty, string.Empty, true);
            }

            string? translation;
            var notFound = false;
            var localized = _stringLocalizer[key, arguments];
            if (localized.ResourceNotFound)
            {
                translation = _provider.GetTranslation(key);
                if (string.IsNullOrEmpty(translation))
                {
                    translation = string.Format(key, arguments);
                    notFound = true;
                }
                else
                {
                    translation = string.Format(translation, arguments);
                }
            }
            else
            {
                translation = localized.Value;
            }

            return new LocalizedString(key, translation, notFound);
        }
    }
}
