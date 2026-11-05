using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpOperationRateLimitingEfCoreTestModule),
    typeof(Management.AbpOperationRateLimitingManagementApplicationModule))]
public class AbpOperationRateLimitingManagementTestModule : AbpModule
{
}
