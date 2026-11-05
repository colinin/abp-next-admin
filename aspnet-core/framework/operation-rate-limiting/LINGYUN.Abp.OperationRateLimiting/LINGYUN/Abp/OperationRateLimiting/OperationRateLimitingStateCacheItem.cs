using Volo.Abp.Caching;

namespace LINGYUN.Abp.OperationRateLimiting;

[CacheName(AbpOperationRateLimitingConsts.StateCacheName)]
public class OperationRateLimitingStateCacheItem
{
    /// <summary>
    /// Serialized state object.
    /// </summary>
    public string State { get; set; } = "{}";
}
