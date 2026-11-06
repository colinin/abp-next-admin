using StackExchange.Redis;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

/// <summary>
/// Redis options of the operation rate limiting store.
/// </summary>
public class AbpOperationRateLimitingRedisOptions
{
    /// <summary>
    /// StackExchange.Redis connection string (used when <see cref="ConfigurationOptions"/> is null).
    /// </summary>
    public string Configuration { get; set; } = "127.0.0.1:6379";

    /// <summary>
    /// Optional prefix of every Redis key, useful to share a database between applications.
    /// </summary>
    public string? InstanceName { get; set; }

    /// <summary>
    /// Advanced connection options. When set, <see cref="Configuration"/> is ignored.
    /// </summary>
    public ConfigurationOptions? ConfigurationOptions { get; set; }
}
