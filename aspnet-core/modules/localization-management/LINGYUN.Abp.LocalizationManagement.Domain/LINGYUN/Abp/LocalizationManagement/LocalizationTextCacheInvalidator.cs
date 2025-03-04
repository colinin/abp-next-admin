using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationTextCacheInvalidator : ILocalEventHandler<EntityChangedEventData<Text>>, ITransientDependency
{
    private readonly IDistributedCache<LocalizationTextCacheItem> _localizationTextCache;
    public LocalizationTextCacheInvalidator(IDistributedCache<LocalizationTextCacheItem> localizationTextCache)
    {
        _localizationTextCache = localizationTextCache;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Text> eventData)
    {
        var cacheKey = LocalizationTextCacheItem.CalculateCacheKey(
            eventData.Entity.ResourceName,
            eventData.Entity.CultureName);

        await _localizationTextCache.RemoveAsync(cacheKey);
    }
}
