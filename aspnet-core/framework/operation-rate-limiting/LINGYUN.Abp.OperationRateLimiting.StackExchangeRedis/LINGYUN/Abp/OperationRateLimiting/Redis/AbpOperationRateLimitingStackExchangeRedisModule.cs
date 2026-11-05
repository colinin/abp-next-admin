using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

/// <summary>
/// Replaces the distributed cache store with a Redis store that executes every algorithm
/// atomically with Lua scripts.
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

        context.Services.Replace(
            ServiceDescriptor.Singleton<IOperationRateLimitingStore, StackExchangeRedisOperationRateLimitingStore>());
    }
}
