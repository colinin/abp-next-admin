using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationManagementExternalContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => true;

    private LocalizationResourceBase _resource;

    private ILocalizationStoreCache _localizationStoreCache;
    private LocalizationStoreCacheInitializeContext _cacheInitializeContext;

    public void Initialize(LocalizationResourceInitializationContext context)
    {
        _resource = context.Resource;

        _cacheInitializeContext = new LocalizationStoreCacheInitializeContext(context.ServiceProvider);
        _localizationStoreCache = context.ServiceProvider.GetRequiredService<ILocalizationStoreCache>();
    }

    public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        AsyncHelper.RunSync(async () => await FillAsync(cultureName, dictionary));
    }

    public async virtual Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        await _localizationStoreCache.InitializeAsync(_cacheInitializeContext);

        var localizedStrings = _localizationStoreCache.GetAllLocalizedStrings(cultureName);

        var localizedStringsInResource = localizedStrings.GetOrDefault(_resource.ResourceName);
        if (localizedStringsInResource != null)
        {
            foreach (var localizedString in localizedStringsInResource)
            {
                dictionary[localizedString.Key] = localizedString.Value;
            }
        }
    }

    public virtual LocalizedString GetOrNull(string cultureName, string name)
    {
        return _localizationStoreCache
            .GetLocalizedStringOrNull(_resource.ResourceName, cultureName, name);
    }

    public virtual Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        var languageInfos = _localizationStoreCache.GetLanguages();

        IEnumerable<string> languages = languageInfos
            .Select(x => x.CultureName)
            .ToList();

        return Task.FromResult(languages);
    }
}
