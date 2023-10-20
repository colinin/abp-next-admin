using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.SettingManagement;
public class DynamicSettingDefinitionStoreCacheInvalidator :
    ILocalEventHandler<EntityChangedEventData<SettingDefinitionRecord>>,
    ITransientDependency
{
    private readonly IDynamicSettingDefinitionStoreInMemoryCache _storeCache;

    private readonly IClock _clock;
    private readonly IDistributedCache _distributedCache;
    private readonly AbpDistributedCacheOptions _cacheOptions;

    public DynamicSettingDefinitionStoreCacheInvalidator(
        IClock clock,
        IDistributedCache distributedCache,
        IDynamicSettingDefinitionStoreInMemoryCache storeCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions)
    {
        _storeCache = storeCache;
        _clock = clock;
        _distributedCache = distributedCache;
        _cacheOptions = cacheOptions.Value;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<SettingDefinitionRecord> eventData)
    {
        await RemoveStampInDistributedCacheAsync();
    }

    protected async virtual Task RemoveStampInDistributedCacheAsync()
    {
        using (await _storeCache.SyncSemaphore.LockAsync())
        {
            var cacheKey = GetCommonStampCacheKey();

            await _distributedCache.RemoveAsync(cacheKey);

            _storeCache.CacheStamp = Guid.NewGuid().ToString();
            _storeCache.LastCheckTime = _clock.Now.AddMinutes(-5);
        }
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{_cacheOptions.KeyPrefix}_AbpInMemorySettingCacheStamp";
    }
}
