using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationCacheInvalidator :
    ILocalEventHandler<EntityChangedEventData<Language>>,
    ILocalEventHandler<EntityChangedEventData<Resource>>,
    ILocalEventHandler<EntityChangedEventData<Text>>,

    IDistributedEventHandler<EntityCreatedEto<TextEto>>,
    IDistributedEventHandler<EntityUpdatedEto<TextEto>>,
    IDistributedEventHandler<EntityDeletedEto<TextEto>>,

    IDistributedEventHandler<EntityCreatedEto<ResourceEto>>,
    IDistributedEventHandler<EntityUpdatedEto<ResourceEto>>,
    IDistributedEventHandler<EntityDeletedEto<ResourceEto>>,

    IDistributedEventHandler<EntityCreatedEto<LanguageEto>>,
    IDistributedEventHandler<EntityUpdatedEto<LanguageEto>>,
    IDistributedEventHandler<EntityDeletedEto<LanguageEto>>,

    ITransientDependency
{
    private readonly IClock _clock;
    private readonly IDistributedCache _distributedCache;
    private readonly ILocalizationStoreCache _storeCache;
    private readonly AbpDistributedCacheOptions _distributedCacheOptions;

    public LocalizationCacheInvalidator(
        IClock clock,
        ILocalizationStoreCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> options)
    {
        _clock = clock;
        _storeCache = storeCache;
        _distributedCache = distributedCache;
        _distributedCacheOptions = options.Value;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Language> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Resource> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Text> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<TextEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<TextEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<TextEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<ResourceEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<ResourceEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<LanguageEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<LanguageEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<LanguageEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<ResourceEto> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    protected async virtual Task RemoveStampInDistributedCacheAsync()
    {
        using (await _storeCache.SyncSemaphore.LockAsync())
        {
            var cacheKey = $"{_distributedCacheOptions.KeyPrefix}_AbpInMemoryLocalizationCacheStamp";

            await _distributedCache.RemoveAsync(cacheKey);

            _storeCache.CacheStamp = Guid.NewGuid().ToString();
            _storeCache.LastCheckTime = _clock.Now.AddMinutes(-5);
        }
    }
}
