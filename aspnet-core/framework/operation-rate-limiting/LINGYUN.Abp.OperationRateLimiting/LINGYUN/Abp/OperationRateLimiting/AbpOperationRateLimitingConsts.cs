namespace LINGYUN.Abp.OperationRateLimiting;

public static class AbpOperationRateLimitingConsts
{
    /// <summary>
    /// Prefix of every cache key produced by the module.
    /// </summary>
    public const string CacheKeyPrefix = "orl";

    /// <summary>
    /// Prefix of every distributed lock name used to update a counter atomically.
    /// </summary>
    public const string LockKeyPrefix = "orl:lock";

    /// <summary>
    /// Cache name of the serialized counter states.
    /// </summary>
    public const string StateCacheName = "OperationRateLimitingState";

    /// <summary>
    /// Cache name of dynamically loaded policy definitions.
    /// </summary>
    public const string PolicyCacheName = "OperationRateLimitingPolicy";

    /// <summary>
    /// Name used as the tenant partition of the host side.
    /// </summary>
    public const string HostTenantName = "host";
}
