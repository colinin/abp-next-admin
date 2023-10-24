using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.LocalizationManagement;

[ExposeServices(
    typeof(ILocalizationStoreCache),
    typeof(LocalizationStoreInMemoryCache))]
public class LocalizationStoreInMemoryCache : ILocalizationStoreCache, ISingletonDependency
{
    public string CacheStamp { get; set; }
    public DateTime? LastCheckTime { get; set; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    protected LocalizationResourceDictionary Resources { get; }
    protected LocalizationLanguageDictionary Languages { get; }
    protected LocalizationDictionaryWithResource LocalizedStrings { get; }

    private readonly IClock _clock;
    private readonly IDistributedCache _distributedCache;
    private readonly IAbpDistributedLock _distributedLock;
    private readonly AbpDistributedCacheOptions _distributedCacheOptions;
    private readonly AbpLocalizationManagementOptions _managementOptions;

    public LocalizationStoreInMemoryCache(
        IClock clock,
        IDistributedCache distributedCache,
        IAbpDistributedLock distributedLock,
        IOptions<AbpDistributedCacheOptions> distributedCacheOptions,
        IOptions<AbpLocalizationManagementOptions> managementOptions)
    {
        _clock = clock;
        _distributedCache = distributedCache;
        _distributedLock = distributedLock;
        _distributedCacheOptions = distributedCacheOptions.Value;
        _managementOptions = managementOptions.Value;

        Resources = new LocalizationResourceDictionary();
        Languages = new LocalizationLanguageDictionary();
        LocalizedStrings = new LocalizationDictionaryWithResource();
    }

    public async virtual Task InitializeAsync(LocalizationStoreCacheInitializeContext context)
    {
        using (await SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync(context);
        }
    }

    public virtual IDictionary<string, LocalizationDictionary> GetAllLocalizedStrings(string cultureName)
    {
        var localizedStrings = new Dictionary<string, LocalizationDictionary>();

        foreach (var resource in Resources)
        {
            var localizedStringsInResource = LocalizedStrings.GetOrDefault(resource.Key);
            if (localizedStringsInResource == null)
            {
                continue;
            }

            var localizedStringsInCurrentCulture = localizedStringsInResource.GetOrDefault(cultureName);
            if (localizedStringsInCurrentCulture == null)
            {
                continue;
            }

            var currentCultureLocalizedStrings = new LocalizationDictionary();

            foreach (var localizedString in localizedStringsInCurrentCulture)
            {
                if (!currentCultureLocalizedStrings.ContainsKey(localizedString.Key))
                {
                    currentCultureLocalizedStrings.Add(localizedString.Key, localizedString.Value);
                }
            }

            localizedStrings[resource.Key] = currentCultureLocalizedStrings;
        }

        return localizedStrings;
    }

    public virtual LocalizedString GetLocalizedStringOrNull(string resourceName, string cultureName, string name)
    {
        var localizedStringsInResource = LocalizedStrings.GetOrDefault(resourceName);
        if (localizedStringsInResource == null)
        {
            return null;
        }

        var currentCultureLocalizedStrings = localizedStringsInResource.GetOrDefault(cultureName);
        if (currentCultureLocalizedStrings == null)
        {
            return null;
        }

        return currentCultureLocalizedStrings.GetOrDefault(name);
    }

    public virtual LocalizationResourceBase GetResourceOrNull(string resourceName)
    {
        return Resources.GetOrDefault(resourceName);
    }

    public virtual IReadOnlyList<LocalizationResourceBase> GetResources()
    {
        return Resources.Values.ToImmutableList();
    }

    public virtual IReadOnlyList<LanguageInfo> GetLanguages()
    {
        return Languages.Values.ToImmutableList();
    }

    protected async virtual Task EnsureCacheIsUptoDateAsync(LocalizationStoreCacheInitializeContext context)
    {
        if (LastCheckTime.HasValue &&
            _clock.Now.Subtract(LastCheckTime.Value).TotalSeconds < 30)
        {
            return;
        }

        var stampInDistributedCache = await GetOrSetStampInDistributedCache();

        if (stampInDistributedCache == CacheStamp)
        {
            LastCheckTime = _clock.Now;
            return;
        }

        await UpdateInMemoryStoreCache(context);

        CacheStamp = stampInDistributedCache;
        LastCheckTime = _clock.Now;
    }

    protected async virtual Task UpdateInMemoryStoreCache(LocalizationStoreCacheInitializeContext context)
    {
        var textRepository = context.ServiceProvider.GetRequiredService<ITextRepository>();
        var languageRepository = context.ServiceProvider.GetRequiredService<ILanguageRepository>();
        var resourceRepository = context.ServiceProvider.GetRequiredService<IResourceRepository>();

        var resourceRecords = await resourceRepository.GetListAsync();
        var languageRecords = await languageRepository.GetActivedListAsync();
        var textRecords = await textRepository.GetListAsync();

        Resources.Clear();
        Languages.Clear();

        foreach (var resourceRecord in resourceRecords)
        {
            Resources[resourceRecord.Name] = new NonTypedLocalizationResource(resourceRecord.Name, resourceRecord.DefaultCultureName);

            var localizedStrings = LocalizedStrings.GetOrDefault(resourceRecord.Name);

            localizedStrings ??= new LocalizationDictionaryWithCulture();
            localizedStrings.Clear();

            // 需要按照不同文化聚合
            foreach (var textRecordByCulture in textRecords.Where(x => x.ResourceName == resourceRecord.Name).GroupBy(x => x.CultureName))
            {
                var currentCultureLocalizedStrings = new LocalizationDictionary();
                foreach (var textRecord in textRecordByCulture)
                {
                    currentCultureLocalizedStrings[textRecord.Key] = new LocalizedString(textRecord.Key, textRecord.Value);
                }
                localizedStrings[textRecordByCulture.Key] = currentCultureLocalizedStrings;
            }

            LocalizedStrings[resourceRecord.Name] = localizedStrings;
        }

        foreach (var language in languageRecords)
        {
            Languages[language.CultureName] = new LanguageInfo(
                language.CultureName,
                language.UiCultureName,
                language.DisplayName,
                language.FlagIcon);
        }
    }

    protected async virtual Task<string> GetOrSetStampInDistributedCache()
    {
        var cacheKey = $"{_distributedCacheOptions.KeyPrefix}_AbpInMemoryLocalizationCacheStamp";

        var stampInDistributedCache = await _distributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        var distributedLockKey = $"{_distributedCacheOptions.KeyPrefix}_AbpLocalizationUpdateLock";
        await using (var commonLockHandle = await _distributedLock
            .TryAcquireAsync(distributedLockKey, _managementOptions.LocalizationCacheStampTimeOut))
        {
            if (commonLockHandle == null)
            {
                /* This request will fail */
                throw new AbpException(
                    "Could not acquire distributed lock for localization stamp check!"
                );
            }

            stampInDistributedCache = await _distributedCache.GetStringAsync(cacheKey);
            if (stampInDistributedCache != null)
            {
                return stampInDistributedCache;
            }

            stampInDistributedCache = Guid.NewGuid().ToString();

            await _distributedCache.SetStringAsync(
                cacheKey,
                stampInDistributedCache,
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = _managementOptions.LocalizationCacheStampExpiration
                }
            );
        }

        return stampInDistributedCache;
    }
}
