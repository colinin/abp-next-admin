using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpOperationRateLimitingApplicationContractsModule))]
public class AbpOperationRateLimitingHttpApiModule : AbpModule
{
}
