using LINGYUN.Abp.Dynamic.Definitions;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;
using LINGYUN.Abp.OperationRateLimiting.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;
using Volo.Abp.Timing;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(AbpTimingModule),
    typeof(AbpMultiTenancyModule),
    typeof(AbpSecurityModule),
    typeof(AbpLocalizationModule),
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpDynamicDefinitionsCoreModule))]
public class AbpOperationRateLimitingModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(OperationRateLimitingInterceptorRegistrar.RegisterIfNeeded);

        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            options.AddAlgorithm<FixedWindowRateLimitingRule>(OperationRateLimitingAlgorithms.FixedWindow);
            options.AddAlgorithm<SlidingWindowRateLimitingRule>(OperationRateLimitingAlgorithms.SlidingWindow);
            options.AddAlgorithm<TokenBucketRateLimitingRule>(OperationRateLimitingAlgorithms.TokenBucket);
            options.AddAlgorithm<ConcurrencyRateLimitingRule>(OperationRateLimitingAlgorithms.Concurrency);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOperationRateLimitingModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<OperationRateLimitingResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/OperationRateLimiting/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(AbpOperationRateLimitingErrorCodes.Namespace, typeof(OperationRateLimitingResource));
        });

        // Policies loaded from a database are merged with the statically defined ones.
        Configure<AbpDynamicDefinitionsOptions>(options =>
        {
            options.MapStrategy<OperationRateLimitingPolicy>(DynamicDefinitionStrategy.Merge);
        });
    }
}
