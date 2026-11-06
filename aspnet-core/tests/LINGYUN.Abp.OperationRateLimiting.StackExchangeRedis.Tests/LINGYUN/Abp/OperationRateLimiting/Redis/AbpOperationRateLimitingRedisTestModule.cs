using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

[DependsOn(
    typeof(AbpOperationRateLimitingTestModule),
    typeof(AbpOperationRateLimitingStackExchangeRedisModule))]
public class AbpOperationRateLimitingRedisTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOperationRateLimitingRedisOptions>(options =>
        {
            options.Configuration = "127.0.0.1:6379";
            // Every test application uses an isolated key prefix.
            options.InstanceName = $"orl-tests-{Guid.NewGuid():N}:";
        });

        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            options.AddPolicy(RedisTestPolicyNames.Atomic, policy =>
                policy.WithFixedWindow(TimeSpan.FromSeconds(10), maxCount: 10)
                    .PartitionByParameter());
        });
    }
}
