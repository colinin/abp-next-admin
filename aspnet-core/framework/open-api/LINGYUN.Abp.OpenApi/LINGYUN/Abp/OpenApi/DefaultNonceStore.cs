using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OpenApi;
public class DefaultNonceStore : INonceStore, ITransientDependency
{
    private const string CacheKeyFormat = "open-api,n:{0}";

    private readonly IDistributedCache<NonceStateCacheItem> _nonceCache;
    private readonly AbpOpenApiOptions _options;

    public DefaultNonceStore(
        IDistributedCache<NonceStateCacheItem> nonceCache, 
        IOptions<AbpOpenApiOptions> options)
    {
        _nonceCache = nonceCache;
        _options = options.Value;
    }

    public async virtual Task<bool> TrySetAsync(string nonce, CancellationToken cancellationToken = default)
    {
        var cacheKey = string.Format(CacheKeyFormat, nonce);

        var cacheItem = await _nonceCache.GetAsync(cacheKey, token: cancellationToken);
        if (cacheItem == null)
        {
            await _nonceCache.SetAsync(
                cacheKey,
                new NonceStateCacheItem(nonce), 
                options: new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _options.RequestNonceExpireIn,
                },
                token: cancellationToken);

            return true;
        }

        return false;
    }
}
