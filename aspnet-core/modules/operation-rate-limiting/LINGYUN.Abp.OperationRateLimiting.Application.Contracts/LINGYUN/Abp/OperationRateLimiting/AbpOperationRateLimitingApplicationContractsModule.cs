using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpOperationRateLimitingDomainSharedModule))]
public class AbpOperationRateLimitingApplicationContractsModule : AbpModule
{
}
