using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.CachingManagement;

public static class ICacheManagerExtensions
{
    public static Task<CackeKeysResponse> GetKeysAsync(
        this ICacheManager cacheManager,
        string prefix = null,
        string filter = null,
        string marker = null,
        CancellationToken cancellationToken = default)
    {
        return cacheManager.GetKeysAsync(
            new GetCacheKeysRequest(prefix, filter, marker),
            cancellationToken);
    }

    public static Task SetAsync(
        this ICacheManager cacheManager,
        string key,
        string value,
        TimeSpan? absExpr = null,
        TimeSpan? sldExpr = null,
        CancellationToken cancellationToken = default)
    {
        return cacheManager.SetAsync(
            new SetCacheRequest(key, value, absExpr, sldExpr),
            cancellationToken);
    }

    public static Task RefreshAsync(
        this ICacheManager cacheManager,
        string key,
        TimeSpan? absExpr = null,
        TimeSpan? sldExpr = null,
        CancellationToken cancellationToken = default)
    {
        return cacheManager.RefreshAsync(
            new RefreshCacheRequest(key, absExpr, sldExpr),
            cancellationToken);
    }
}
