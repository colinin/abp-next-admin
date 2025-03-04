using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationLanguageCacheInvalidator: ILocalEventHandler<EntityChangedEventData<Language>>, ITransientDependency
{
    private readonly IDistributedCache<LocalizationLanguageCacheItem> _localizationLanguageCache;
    public LocalizationLanguageCacheInvalidator(IDistributedCache<LocalizationLanguageCacheItem> localizationLanguageCache)
    {
        _localizationLanguageCache = localizationLanguageCache;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Language> eventData)
    {
        await _localizationLanguageCache.RemoveAsync(LocalizationLanguageCacheItem.CacheKey);
    }
}
