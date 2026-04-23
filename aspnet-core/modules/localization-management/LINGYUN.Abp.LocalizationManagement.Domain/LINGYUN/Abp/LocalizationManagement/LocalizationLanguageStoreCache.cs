using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationLanguageStoreCache : ILocalizationLanguageStoreCache, ITransientDependency
{
    protected IDistributedCache<LocalizationLanguageCacheItem> LanguageCache { get; }
    protected ILanguageRepository LanguageRepository { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpLocalizationOptions Options { get; }

    public LocalizationLanguageStoreCache(
        IDistributedCache<LocalizationLanguageCacheItem> languageCache, 
        ILanguageRepository languageRepository,
        IAbpDistributedLock distributedLock,
        IOptions<AbpLocalizationOptions> options)
    {
        LanguageCache = languageCache;
        LanguageRepository = languageRepository;
        DistributedLock  = distributedLock;
        Options = options.Value;
    }

    [DisableEntityChangeTracking]
    public async virtual Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
    {
        var cacheItem = await GetCacheItemAsync();

        return cacheItem?.Languages.ToImmutableList() ?? [..Options.Languages];
    }

    internal async Task UpdateCache()
    {
        await CreateCacheItemAsync();
    }

    protected async virtual Task<LocalizationLanguageCacheItem> GetCacheItemAsync()
    {
        var cacheItem = await LanguageCache.GetAsync(LocalizationLanguageCacheItem.CacheKey);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        return await CreateCacheItemAsync();
    }

    protected async virtual Task<LocalizationLanguageCacheItem> CreateCacheItemAsync()
    {
        await using var handle = await DistributedLock.TryAcquireAsync($"{nameof(LocalizationLanguageStoreCache)}_{nameof(GetCacheItemAsync)}");

        if (handle == null)
        {
            return null;
        }

        var languages = await LanguageRepository.GetListAsync();

        var cacheItem = new LocalizationLanguageCacheItem(
            languages.Select(x =>
                new LanguageInfo(x.CultureName, x.UiCultureName, x.DisplayName))
            .ToList());

        await LanguageCache.SetAsync(LocalizationLanguageCacheItem.CacheKey, cacheItem);

        return cacheItem;
    }
}
