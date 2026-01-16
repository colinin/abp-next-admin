using LINGYUN.Abp.AI;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AIManagement;

[DependsOn(
    typeof(AbpAIManagementDomainSharedModule),
    typeof(AbpAICoreModule),
    typeof(AbpCachingModule),
    typeof(AbpMapperlyModule),
    typeof(AbpDddDomainModule))]
public class AbpAIManagementDomainModule : AbpModule
{
}
