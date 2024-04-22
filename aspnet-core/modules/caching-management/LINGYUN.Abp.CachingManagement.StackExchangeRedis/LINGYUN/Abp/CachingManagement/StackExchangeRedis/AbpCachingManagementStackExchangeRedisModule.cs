using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.CachingManagement.StackExchangeRedis;

[DependsOn(
    typeof(AbpCachingManagementDomainModule),
    typeof(AbpCachingStackExchangeRedisModule))]
public class AbpCachingManagementStackExchangeRedisModule : AbpModule
{
}
