using LINGYUN.Abp.Saas;
using Volo.Abp.Caching;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MultiTenancy.Saas;

[DependsOn(typeof(AbpCachingModule))]
[DependsOn(typeof(AbpEventBusModule))]
[DependsOn(typeof(AbpSaasHttpApiClientModule))]
public class AbpMultiTenancySaasModule : AbpModule
{
}
