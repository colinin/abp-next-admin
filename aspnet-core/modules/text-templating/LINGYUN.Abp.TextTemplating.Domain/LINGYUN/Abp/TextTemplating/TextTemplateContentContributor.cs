using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateContentContributor : ITemplateContentContributor, ITransientDependency
{
    protected AbpTextTemplatingCachingOptions TemplatingCachingOptions { get; }
    protected IDistributedCache<TemplateContentCacheItem> TextTemplateContentCache { get; }

    public TextTemplateContentContributor(
        IDistributedCache<TemplateContentCacheItem> textTemplateContentCache,
        IOptions<AbpTextTemplatingCachingOptions> templatingCachingOptions)
    {
        TextTemplateContentCache = textTemplateContentCache;
        TemplatingCachingOptions = templatingCachingOptions.Value;
    }

    public async virtual Task<string> GetOrNullAsync(TemplateContentContributorContext context)
    {
        var cacheKey = TemplateContentCacheItem.CalculateCacheKey(context.TemplateDefinition.Name, context.Culture);

        var cacheItem = await TextTemplateContentCache.GetOrAddAsync(cacheKey,
            () => CreateTemplateContentCache(context),
            () => CreateTemplateContentCacheOptions());

        return cacheItem?.Content;
    }

    protected async virtual Task<TemplateContentCacheItem> CreateTemplateContentCache(TemplateContentContributorContext context)
    {
        var repository = context.ServiceProvider.GetRequiredService<ITextTemplateRepository>();
        var template = await repository.FindByNameAsync(context.TemplateDefinition.Name, context.Culture);

        return new TemplateContentCacheItem(
            template?.Name,
            template?.Content,
            template?.Culture);
    }

    protected DistributedCacheEntryOptions CreateTemplateContentCacheOptions()
    {
        return new DistributedCacheEntryOptions
        {
            SlidingExpiration = TemplatingCachingOptions.MinimumCacheDuration,
            AbsoluteExpirationRelativeToNow = TemplatingCachingOptions.MaximumCacheDuration,
        };
    }
}
