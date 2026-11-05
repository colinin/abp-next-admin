using Volo.Abp.Application;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpEventBusModule),
    typeof(AbpOperationRateLimitingDomainModule),
    typeof(AbpOperationRateLimitingApplicationContractsModule))]
public class AbpOperationRateLimitingApplicationModule : AbpModule
{
}
