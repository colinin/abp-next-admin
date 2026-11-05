using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(AbpOperationRateLimitingModule),
    typeof(AbpOperationRateLimitingDomainSharedModule))]
public class AbpOperationRateLimitingDomainModule : AbpModule
{
}
