using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationLanguageStoreCache : ILocalizationLanguageStoreCache, ITransientDependency
{
    protected IDistributedCache<LocalizationLanguageCacheItem> LanguageCache { get; }
    protected ILanguageRepository LanguageRepository { get; }

    public LocalizationLanguageStoreCache(
        IDistributedCache<LocalizationLanguageCacheItem> languageCache, 
        ILanguageRepository languageRepository)
    {
        LanguageCache = languageCache;
        LanguageRepository = languageRepository;
    }

    [DisableEntityChangeTracking]
    public async virtual Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
    {
        var cacheItem = await GetCacheItemAsync();

        return cacheItem.Languages.ToImmutableList();
    }

    protected async virtual Task<LocalizationLanguageCacheItem> GetCacheItemAsync()
    {
        var cacheItem = await LanguageCache.GetAsync(LocalizationLanguageCacheItem.CacheKey);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        var languages = await LanguageRepository.GetListAsync();

        cacheItem = new LocalizationLanguageCacheItem(
            languages.Select(x => 
                new LanguageInfo(x.CultureName, x.UiCultureName, x.DisplayName))
            .ToList());

        return cacheItem;
    }
}
