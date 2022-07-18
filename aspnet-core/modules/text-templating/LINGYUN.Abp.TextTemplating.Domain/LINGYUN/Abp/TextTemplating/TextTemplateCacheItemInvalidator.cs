using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateCacheItemInvalidator :
    ILocalEventHandler<EntityChangedEventData<TextTemplate>>,
    ILocalEventHandler<EntityDeletedEventData<TextTemplate>>,
    ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }

    protected IDistributedCache<TemplateContentCacheItem> Cache { get; }

    public TextTemplateCacheItemInvalidator(IDistributedCache<TemplateContentCacheItem> cache, ICurrentTenant currentTenant)
    {
        Cache = cache;
        CurrentTenant = currentTenant;
    }

    public async virtual Task HandleEventAsync(EntityChangedEventData<TextTemplate> eventData)
    {
        await RemoveCacheItemAsync(eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityDeletedEventData<TextTemplate> eventData)
    {
        await RemoveCacheItemAsync(eventData.Entity);
    }

    protected async virtual Task RemoveCacheItemAsync(TextTemplate template)
    {
        var cacheKey = CalculateCacheKey(
            template.Name,
            template.Culture
        );

        using (CurrentTenant.Change(template.TenantId))
        {
            await Cache.RemoveAsync(cacheKey);
        }
    }

    protected virtual string CalculateCacheKey(string name, string culture = null)
    {
        return TemplateContentCacheItem.CalculateCacheKey(name, culture);
    }
}
