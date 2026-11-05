using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpOperationRateLimitingEfCoreTestModule),
    typeof(AbpOperationRateLimitingApplicationModule))]
public class AbpOperationRateLimitingManagementTestModule : AbpModule
{
}
