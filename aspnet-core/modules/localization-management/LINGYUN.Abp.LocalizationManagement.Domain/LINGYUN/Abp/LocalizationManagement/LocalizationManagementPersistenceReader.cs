using LINGYUN.Abp.Localization.Persistence;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(ILocalizationPersistenceReader),
    typeof(LocalizationManagementPersistenceReader))]
public class LocalizationManagementPersistenceReader : ILocalizationPersistenceReader, ITransientDependency
{
    private readonly ILocalizationStoreCache _localizationStoreCache;
    private readonly LocalizationStoreCacheInitializeContext _cacheInitializeContext;

    public LocalizationManagementPersistenceReader(
        IServiceProvider serviceProvider,
        ILocalizationStoreCache localizationStoreCache)
    {
        _localizationStoreCache = localizationStoreCache;
        _cacheInitializeContext = new LocalizationStoreCacheInitializeContext(serviceProvider);
    }

    public virtual void Fill(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        AsyncHelper.RunSync(async () => await FillAsync(resourceName, cultureName, dictionary));
    }

    public async virtual Task FillAsync(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        await _localizationStoreCache.InitializeAsync(_cacheInitializeContext);

        var localizedStrings = _localizationStoreCache.GetAllLocalizedStrings(cultureName);

        var localizedStringsInResource = localizedStrings.GetOrDefault(resourceName);
        if (localizedStringsInResource != null)
        {
            foreach (var localizedString in localizedStringsInResource)
            {
                dictionary[localizedString.Key] = localizedString.Value;
            }
        }
    }

    public virtual LocalizedString GetOrNull(string resourceName, string cultureName, string name)
    {
        return _localizationStoreCache.GetLocalizedStringOrNull(resourceName, cultureName, name);
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
