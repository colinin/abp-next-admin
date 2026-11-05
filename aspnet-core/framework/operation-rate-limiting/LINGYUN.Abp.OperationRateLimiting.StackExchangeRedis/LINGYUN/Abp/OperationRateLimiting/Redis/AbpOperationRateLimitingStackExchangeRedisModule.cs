using LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.Concurrency;
using LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.FixedWindow;
using LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.TokenBucket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

/// <summary>
/// Replaces the built-in rules with Redis Lua implementations and the distributed cache state
/// accessor with a Redis compare-and-set implementation.
/// </summary>
[DependsOn(typeof(AbpOperationRateLimitingModule))]
public class AbpOperationRateLimitingStackExchangeRedisModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration;

        Configure<AbpOperationRateLimitingRedisOptions>(configuration.GetSection("OperationRateLimiting:Redis"));

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOperationRateLimitingStackExchangeRedisModule>();
        });

        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            options.AddAlgorithm<RedisFixedWindowRateLimitingRule>(OperationRateLimitingAlgorithms.FixedWindow);
            options.AddAlgorithm<RedisSlidingWindowRateLimitingRule>(OperationRateLimitingAlgorithms.SlidingWindow);
            options.AddAlgorithm<RedisTokenBucketRateLimitingRule>(OperationRateLimitingAlgorithms.TokenBucket);
            options.AddAlgorithm<RedisConcurrencyRateLimitingRule>(OperationRateLimitingAlgorithms.Concurrency);
        });

        context.Services.AddSingleton<StackExchangeRedisOperationRateLimitingExecutor>();

        context.Services.Replace(
            ServiceDescriptor.Singleton<IOperationRateLimitingStateAccessor, StackExchangeRedisOperationRateLimitingStateAccessor>());
    }
}
