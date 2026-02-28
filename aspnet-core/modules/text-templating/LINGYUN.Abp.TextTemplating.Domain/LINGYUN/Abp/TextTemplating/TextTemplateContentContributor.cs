using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateContentContributor : ITemplateContentContributor, ITransientDependency
{
    public ILogger<TextTemplateContentContributor> Logger { protected get; set; }
    protected AbpTextTemplatingCachingOptions TemplatingCachingOptions { get; }
    protected IDistributedCache<TextTemplateContentCacheItem> TextTemplateContentCache { get; }

    public TextTemplateContentContributor(
        IDistributedCache<TextTemplateContentCacheItem> textTemplateContentCache,
        IOptions<AbpTextTemplatingCachingOptions> templatingCachingOptions)
    {
        TextTemplateContentCache = textTemplateContentCache;
        TemplatingCachingOptions = templatingCachingOptions.Value;

        Logger = NullLogger<TextTemplateContentContributor>.Instance;
    }

    public async virtual Task<string> GetOrNullAsync(TemplateContentContributorContext context)
    {
        return (await GetCacheItemAsync(context)).Content;
    }

    protected async virtual Task<TextTemplateContentCacheItem> GetCacheItemAsync(TemplateContentContributorContext context)
    {
        var culture = context.TemplateDefinition.IsInlineLocalized ? null : context.Culture;
        var cacheKey = TextTemplateContentCacheItem.CalculateCacheKey(context.TemplateDefinition.Name, culture);

        Logger.LogDebug($"TextTemplateContentContributor.GetCacheItemAsync: {cacheKey}");

        var cacheItem = await TextTemplateContentCache.GetAsync(cacheKey);

        if (cacheItem != null)
        {
            Logger.LogDebug($"TextTemplateContent found in the cache: {cacheKey}");
            return cacheItem;
        }

        Logger.LogDebug($"TextTemplateContent not found in the cache: {cacheKey}");

        var repository = context.ServiceProvider.GetRequiredService<ITextTemplateRepository>();
        var template = await repository.FindByNameAsync(context.TemplateDefinition.Name, culture);
        if (template == null && !culture.IsNullOrWhiteSpace())
        {
            template = await repository.FindByNameAsync(context.TemplateDefinition.Name, context.TemplateDefinition.DefaultCultureName);
        }

        cacheItem = new TextTemplateContentCacheItem(
            template?.Name,
            template?.Content,
            template?.Culture);

        Logger.LogDebug($"TextTemplateContent set cache item: {cacheKey}");

        await TextTemplateContentCache.SetAsync(cacheKey, cacheItem, CreateTemplateContentCacheOptions());

        return cacheItem;
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
