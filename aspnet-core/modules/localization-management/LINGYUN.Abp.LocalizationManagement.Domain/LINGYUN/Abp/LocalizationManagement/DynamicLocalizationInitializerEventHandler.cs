using LINGYUN.Abp.LocalizationManagement.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.LocalizationManagement;

public class DynamicLocalizationInitializerEventHandler :
    IDistributedEventHandler<DynamicLanguageInitializerEto>,
    IDistributedEventHandler<DynamicResourceInitializerEto>,
    IDistributedEventHandler<DynamicTextInitializerEto>,
    ILocalEventHandler<DynamicLanguageRefreshEventData>,
    ILocalEventHandler<DynamicResourceRefreshEventData>,
    ILocalEventHandler<DynamicTextRefreshEventData>,
    ITransientDependency
{
    public ILogger<StaticLocalizationSaver> Logger { protected get; set; }

    protected IDistributedCache<LocalizationResourceCacheItem> ResourceCache { get; }
    protected IDistributedCache<LocalizationResourcesCacheItem> ResourcesCache { get; }
    protected IDistributedCache<LocalizationLanguageCacheItem> LanguageCache { get; }
    protected IDistributedCache<LocalizationTextCacheItem> LocalizationTextCache { get; }

    protected IDistributedCache<DynamicLanguageInitializerCacheItem> DynamicLanguageCache { get; }
    protected IDistributedCache<DynamicResourceInitializerCacheItem> DynamicResourceCache { get; }
    protected IDistributedCache<DynamicTextInitializerCacheItem> DynamicTextCache { get; }

    protected AbpLocalizationManagementOptions LocalizationManagementOptions { get; }

    protected IGuidGenerator GuidGenerator { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected ILanguageRepository LanguageRepository { get; }
    protected IResourceRepository ResourceRepository { get; }
    protected ITextRepository TextRepository { get; }

    public DynamicLocalizationInitializerEventHandler(
        IDistributedCache<LocalizationResourceCacheItem> resourceCache,
        IDistributedCache<LocalizationResourcesCacheItem> resourcesCache,
        IDistributedCache<LocalizationLanguageCacheItem> languageCache,
        IDistributedCache<LocalizationTextCacheItem> localizationTextCache,
        IDistributedCache<DynamicLanguageInitializerCacheItem> dynamicLanguageCache,
        IDistributedCache<DynamicResourceInitializerCacheItem> dynamicResourceCache,
        IDistributedCache<DynamicTextInitializerCacheItem> dynamicTextCache,
        IOptions<AbpLocalizationManagementOptions> localizationManagementOptions,
        IGuidGenerator guidGenerator,
        IAbpDistributedLock distributedLock, 
        IDistributedEventBus distributedEventBus, 
        IUnitOfWorkManager unitOfWorkManager,
        ILanguageRepository languageRepository, 
        IResourceRepository resourceRepository, 
        ITextRepository textRepository)
    {
        ResourceCache = resourceCache;
        ResourcesCache = resourcesCache;
        LanguageCache = languageCache;
        LocalizationTextCache = localizationTextCache;
        DynamicLanguageCache = dynamicLanguageCache;
        DynamicResourceCache = dynamicResourceCache;
        DynamicTextCache = dynamicTextCache;
        LocalizationManagementOptions = localizationManagementOptions.Value;
        GuidGenerator = guidGenerator;
        DistributedLock = distributedLock;
        DistributedEventBus = distributedEventBus;
        UnitOfWorkManager = unitOfWorkManager;
        LanguageRepository = languageRepository;
        ResourceRepository = resourceRepository;
        TextRepository = textRepository;

        Logger = NullLogger<StaticLocalizationSaver>.Instance;
    }

    [DisableEntityChangeTracking]
    public async virtual Task HandleEventAsync(DynamicLanguageRefreshEventData eventData)
    {
        Logger.LogInformation("Languages have changed. Refresh the language cache.");

        await RefreshLanguagesCacheAsync();

        await DistributedEventBus.PublishAsync(new DynamicLocalizationChangedEto());

        Logger.LogInformation("Language cache has been refreshed.");
    }

    [DisableEntityChangeTracking]
    public async virtual Task HandleEventAsync(DynamicLanguageInitializerEto eventData)
    {
        if (!LocalizationManagementOptions.SaveStaticLocalizationsToDatabase ||
            !LocalizationManagementOptions.IsDynamicLocalizationInitializerHost)
        {
            return;
        }

        Logger.LogDebug("Waiting to acquire the distributed lock for saving static languages...");

        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            $"{nameof(DynamicLocalizationInitializerEventHandler)}_{nameof(DynamicLanguageInitializerEto)}",
            TimeSpan.FromSeconds(5));
        if (applicationLockHandle == null)
        {
            throw new AbpException("Failed to acquire the initialization lock for the localization languages!");
        }

        using var unitOfWork = UnitOfWorkManager.Begin(true, true);

        var cacheItems = await DynamicLanguageCache.GetManyAsync(eventData.Keys);
        if (cacheItems == null)
        {
            return;
        }

        await SaveLanguagesAsync(cacheItems.Select(kv => kv.Value).ToArray());

        Logger.LogInformation("Refresh language cache items.");
        await RefreshLanguagesCacheAsync();

        await DynamicLanguageCache.RemoveManyAsync(eventData.Keys);

        await DistributedEventBus.PublishAsync(new DynamicLocalizationChangedEto());

        await unitOfWork.CompleteAsync();

        Logger.LogInformation("Completed to save static languages.");
    }

    [DisableEntityChangeTracking]
    public async virtual Task HandleEventAsync(DynamicResourceRefreshEventData eventData)
    {
        Logger.LogInformation("Resources have changed. Refresh the resource cache.");

        await RefreshResourcesCacheAsync();

        await DistributedEventBus.PublishAsync(new DynamicLocalizationChangedEto());

        Logger.LogInformation("Resource cache has been refreshed.");
    }

    [DisableEntityChangeTracking]
    public async virtual Task HandleEventAsync(DynamicResourceInitializerEto eventData)
    {
        if (!LocalizationManagementOptions.SaveStaticLocalizationsToDatabase ||
            !LocalizationManagementOptions.IsDynamicLocalizationInitializerHost)
        {
            return;
        }

        Logger.LogDebug("Waiting to acquire the distributed lock for saving static resources...");

        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            $"{nameof(DynamicLocalizationInitializerEventHandler)}_{nameof(DynamicResourceInitializerEto)}",
            TimeSpan.FromSeconds(5));
        if (applicationLockHandle == null)
        {
            throw new AbpException("Failed to acquire the initialization lock for the localization resources!");
        }

        using var unitOfWork = UnitOfWorkManager.Begin(true, true);

        var cacheItems = await DynamicResourceCache.GetManyAsync(eventData.Keys);
        if (cacheItems == null)
        {
            return;
        }

        await SaveResourcesAsync(cacheItems.Select(kv => kv.Value).ToArray());

        Logger.LogInformation("Refresh resource cache items.");
        await RefreshResourcesCacheAsync();

        await DynamicResourceCache.RemoveManyAsync(eventData.Keys);

        await DistributedEventBus.PublishAsync(new DynamicLocalizationChangedEto());

        await unitOfWork.CompleteAsync();

        Logger.LogInformation("Completed to save static resources.");
    }

    [DisableEntityChangeTracking]
    public async virtual Task HandleEventAsync(DynamicTextRefreshEventData eventData)
    {
        Logger.LogInformation("Texts have changed. Refresh the text cache.");

        var setTexts = new Dictionary<string, string>();
        var allTexts = await TextRepository.GetListAsync(eventData.ResourceName, eventData.CultureName);
        foreach (var text in allTexts)
        {
            setTexts[text.Key] = text.Value;
        }

        var textCacheKey = LocalizationTextCacheItem.CalculateCacheKey(eventData.ResourceName, eventData.CultureName);
        var textCacheItem = new LocalizationTextCacheItem(eventData.ResourceName, eventData.CultureName, setTexts);

        await LocalizationTextCache.SetAsync(textCacheKey, textCacheItem);

        await DistributedEventBus.PublishAsync(new DynamicLocalizationChangedEto());

        Logger.LogInformation("Text cache has been refreshed.");
    }

    [DisableEntityChangeTracking]
    public async virtual Task HandleEventAsync(DynamicTextInitializerEto eventData)
    {
        if (!LocalizationManagementOptions.SaveStaticLocalizationsToDatabase ||
            !LocalizationManagementOptions.IsDynamicLocalizationInitializerHost)
        {
            return;
        }

        Logger.LogDebug("Waiting to acquire the distributed lock for saving static texts...");

        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            $"{nameof(DynamicLocalizationInitializerEventHandler)}_{nameof(DynamicTextInitializerEto)}",
            TimeSpan.FromSeconds(5));
        if (applicationLockHandle == null)
        {
            throw new AbpException("Failed to acquire the initialization lock for the localization texts!");
        }

        using var unitOfWork = UnitOfWorkManager.Begin(true, true);

        var cacheItemDics = await DynamicTextCache.GetManyAsync(eventData.Keys);
        var cacheItems = cacheItemDics.Select(kv => kv.Value).ToArray();
        if (cacheItems == null)
        {
            return;
        }

        await SaveTextsAsync(cacheItems);

        Logger.LogInformation("Refresh text cache items.");
        await RefreshTextsCacheAsync(cacheItems);

        await DynamicResourceCache.RemoveManyAsync(eventData.Keys);

        await DistributedEventBus.PublishAsync(new DynamicLocalizationChangedEto());

        await unitOfWork.CompleteAsync();

        Logger.LogInformation("Completed to save static texts.");
    }

    private async Task SaveLanguagesAsync(DynamicLanguageInitializerCacheItem[] cacheItems)
    {
        var newLanguages = new List<Language>();
        var updateLanguages = new List<Language>();
        var existsLanguages = await LanguageRepository.GetListAsync();
        foreach (var cacheItem in cacheItems)
        {
            var existsLanguage = existsLanguages.FirstOrDefault(x => x.CultureName == cacheItem.CultureName);
            if (existsLanguage == null)
            {
                newLanguages.Add(
                    new Language(
                        GuidGenerator.Create(),
                        cacheItem.CultureName,
                        cacheItem.UiCultureName,
                        cacheItem.DisplayName,
                        cacheItem.TwoLetterISOLanguageName));
            }
            else
            {
                var isUpdated = false;
                if (existsLanguage.DisplayName != cacheItem.DisplayName)
                {
                    isUpdated = true;
                    existsLanguage.SetDisplayName(cacheItem.DisplayName);
                }
                if (existsLanguage.TwoLetterISOLanguageName != cacheItem.TwoLetterISOLanguageName)
                {
                    isUpdated = true;
                    existsLanguage.SetTwoLetterISOLanguageName(cacheItem.TwoLetterISOLanguageName);
                }
                if (isUpdated)
                {
                    updateLanguages.Add(existsLanguage);
                }
            }
        }

        if (newLanguages.Count > 0)
        {
            Logger.LogInformation("Saved {0} new languages.", newLanguages.Count);
            await LanguageRepository.InsertManyAsync(newLanguages, autoSave: true);
        }

        if (updateLanguages.Count > 0)
        {
            Logger.LogInformation("Update {0} changed languages.", updateLanguages.Count);
            await LanguageRepository.UpdateManyAsync(updateLanguages, autoSave: true);
        }
    }

    private async Task RefreshLanguagesCacheAsync()
    {
        var languages = await LanguageRepository.GetListAsync();

        var languageCacheItem = new LocalizationLanguageCacheItem(
            languages.Select(x =>
                new LanguageInfo(x.CultureName, x.UiCultureName, x.DisplayName))
            .ToList());

        await LanguageCache.SetAsync(LocalizationLanguageCacheItem.CacheKey, languageCacheItem);
    }

    private async Task SaveResourcesAsync(DynamicResourceInitializerCacheItem[] cacheItems)
    {
        var newResources = new List<Resource>();
        var updateResources = new List<Resource>();
        var existsResources = await ResourceRepository.GetListAsync();

        foreach (var cacheItem in cacheItems)
        {
            var existsResource = existsResources.FirstOrDefault(x => x.Name == cacheItem.ResourceName);
            if (existsResource == null)
            {
                newResources.Add(
                    new Resource(
                        GuidGenerator.Create(),
                        cacheItem.ResourceName,
                        cacheItem.ResourceName,
                        cacheItem.ResourceName,
                        cacheItem.DefaultCultureName));
            }
            else if (existsResource.DefaultCultureName != cacheItem.DefaultCultureName)
            {
                existsResource.SetDefaultCultureName(cacheItem.DefaultCultureName);
                updateResources.Add(existsResource);
            }
        }

        if (newResources.Count > 0)
        {
            Logger.LogInformation("Saved {0} new resources.", newResources.Count);
            await ResourceRepository.InsertManyAsync(newResources, autoSave: true);
        }

        if (updateResources.Count > 0)
        {
            Logger.LogInformation("Update {0} changed resources.", updateResources.Count);
            await ResourceRepository.UpdateManyAsync(updateResources, autoSave: true);
        }
    }

    private async Task RefreshResourcesCacheAsync()
    {
        var resources = await ResourceRepository.GetListAsync();

        var allResourceCacheItems = new List<LocalizationResourceCacheItem>();
        var allResourceCacheItemDics = new Dictionary<string, LocalizationResourceCacheItem>();
        foreach (var resource in resources)
        {
            var resourceCacheItem = new LocalizationResourceCacheItem(resource.Name, resource.DefaultCultureName)
            {
                IsEnabled = resource.Enable
            };
            allResourceCacheItems.Add(resourceCacheItem);
            allResourceCacheItemDics[resource.Name] = resourceCacheItem;
        }

        await ResourcesCache.SetAsync(
            LocalizationResourcesCacheItem.CacheKey,
            new LocalizationResourcesCacheItem(allResourceCacheItems));

        await ResourceCache.SetManyAsync(allResourceCacheItemDics);
    }

    private async Task SaveTextsAsync(DynamicTextInitializerCacheItem[] cacheItems)
    {
        var newTexts = new List<Text>();
        var updateTexts = new List<Text>();

        foreach (var cacheItem in cacheItems)
        {
            var existsTexts = await TextRepository.GetListAsync(
                cacheItem.ResourceName,
                cacheItem.CultureName);

            foreach (var text in cacheItem.Texts)
            {
                var existsText = existsTexts.FirstOrDefault(x => x.Key == text.Key);
                if (existsText == null)
                {
                    newTexts.Add(
                        new Text(
                            cacheItem.ResourceName,
                            cacheItem.CultureName,
                            text.Key,
                            text.Value));
                }
                else if (existsText.Value != text.Value)
                {
                    existsText.SetValue(text.Value);
                    updateTexts.Add(existsText);
                }
            }
        }

        if (newTexts.Count > 0)
        {
            Logger.LogInformation("Saved {0} new texts.", newTexts.Count);
            await TextRepository.InsertManyAsync(newTexts, autoSave: true);
        }

        if (updateTexts.Count > 0)
        {
            Logger.LogInformation("Update {0} changed texts.", updateTexts.Count);
            await TextRepository.UpdateManyAsync(updateTexts, autoSave: true);
        }
    }

    private async Task RefreshTextsCacheAsync(DynamicTextInitializerCacheItem[] cacheItems)
    {
        foreach (var cacheItem in cacheItems)
        {
            var setTexts = new Dictionary<string, string>();
            var allTexts = await TextRepository.GetListAsync(cacheItem.ResourceName, cacheItem.CultureName);
            foreach (var text in allTexts)
            {
                setTexts[text.Key] = text.Value;
            }

            var textCacheKey = LocalizationTextCacheItem.CalculateCacheKey(cacheItem.ResourceName, cacheItem.CultureName);
            var textCacheItem = new LocalizationTextCacheItem(cacheItem.ResourceName, cacheItem.CultureName, setTexts);

            await LocalizationTextCache.SetAsync(textCacheKey, textCacheItem);
        }
    }
}
