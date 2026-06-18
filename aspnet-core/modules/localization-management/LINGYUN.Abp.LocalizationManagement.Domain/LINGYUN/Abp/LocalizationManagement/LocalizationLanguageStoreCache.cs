using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationLanguageStoreCache : ILocalizationLanguageStoreCache, ITransientDependency
{
    protected IDistributedCache<LocalizationLanguageCacheItem> LanguageCache { get; }
    protected AbpLocalizationOptions Options { get; }

    public LocalizationLanguageStoreCache(
        IDistributedCache<LocalizationLanguageCacheItem> languageCache,
        IOptions<AbpLocalizationOptions> options)
    {
        LanguageCache = languageCache;
        Options = options.Value;
    }

    public async virtual Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
    {
        var cacheItem = await LanguageCache.GetAsync(LocalizationLanguageCacheItem.CacheKey);

        return cacheItem?.Languages.ToImmutableList() ?? [..Options.Languages];
    }
}
