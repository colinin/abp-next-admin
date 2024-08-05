using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Identity.Session;
public class DefaultIdentitySessionCache : IIdentitySessionCache, ITransientDependency
{
    public ILogger<DefaultIdentitySessionCache> Logger { protected get; set; }
    protected IDistributedCache<IdentitySessionCacheItem> Cache { get; }

    public DefaultIdentitySessionCache(IDistributedCache<IdentitySessionCacheItem> cache)
    {
        Cache = cache;

        Logger = NullLogger<DefaultIdentitySessionCache>.Instance;
    }

    public async virtual Task<IdentitySessionCacheItem> GetAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"Get user session cache for: {sessionId}");
        var cacheKey = IdentitySessionCacheItem.CalculateCacheKey(sessionId);

        return await Cache.GetAsync(cacheKey, token: cancellationToken);
    }

    public async virtual Task RefreshAsync(string sessionId, IdentitySessionCacheItem cacheItem, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"Refresh user session cache for: {sessionId}");

        var cacheKey = IdentitySessionCacheItem.CalculateCacheKey(sessionId);

        await Cache.SetAsync(cacheKey, cacheItem, token: cancellationToken);
    }

    public async virtual Task RemoveAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug($"Remove user session cache for: {sessionId}");

        var cacheKey = IdentitySessionCacheItem.CalculateCacheKey(sessionId);

        await Cache.RemoveAsync(cacheKey, token: cancellationToken);
    }
}
