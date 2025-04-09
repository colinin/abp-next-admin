using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.LocalizationManagement.External;

public class LocalizationResourceCacheInvalidator : 
    ILocalEventHandler<EntityChangedEventData<Resource>>, 
    ITransientDependency
{
    private readonly IExternalLocalizationTextStoreCache _externalLocalizationTextStoreCache;
    private readonly ILocalizationLanguageStoreCache _localizationLanguageStoreCache;
    private readonly IExternalLocalizationStoreCache _externalLocalizationStoreCache;
    public LocalizationResourceCacheInvalidator(
        IExternalLocalizationTextStoreCache externalLocalizationTextStoreCache,
        ILocalizationLanguageStoreCache localizationLanguageStoreCache,
        IExternalLocalizationStoreCache externalLocalizationStoreCache)
    {
        _externalLocalizationTextStoreCache = externalLocalizationTextStoreCache;
        _localizationLanguageStoreCache = localizationLanguageStoreCache;
        _externalLocalizationStoreCache = externalLocalizationStoreCache;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<Resource> eventData)
    {
        await _externalLocalizationStoreCache.RemoveAsync(eventData.Entity.Name);
;        
        var languages = await _localizationLanguageStoreCache.GetLanguagesAsync();

        foreach (var language in languages)
        {
            await _externalLocalizationTextStoreCache.RemoveAsync(eventData.Entity.Name, language.CultureName);
        }
    }
}
