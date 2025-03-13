using LINGYUN.Abp.LocalizationManagement.External;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationTextCacheInvalidator : ILocalEventHandler<EntityChangedEventData<Text>>, ITransientDependency
{
    private readonly IDistributedCache<LocalizationTextCacheItem> _localizationTextCache;
    private readonly IExternalLocalizationTextStoreCache _externalLocalizationTextStoreCache;
    public LocalizationTextCacheInvalidator(
        IDistributedCache<LocalizationTextCacheItem> localizationTextCache,
        IExternalLocalizationTextStoreCache externalLocalizationTextStoreCache)
    {
        _externalLocalizationTextStoreCache = externalLocalizationTextStoreCache;
        _localizationTextCache = localizationTextCache;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Text> eventData)
    {
        var cacheKey = LocalizationTextCacheItem.CalculateCacheKey(
            eventData.Entity.ResourceName,
            eventData.Entity.CultureName);

        await _localizationTextCache.RemoveAsync(cacheKey);

        await _externalLocalizationTextStoreCache.RemoveAsync(eventData.Entity.ResourceName, eventData.Entity.CultureName);
    }
}
