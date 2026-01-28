using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AIManagement;

[DependsOn(
    typeof(AbpAIManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule))]
public class AbpAIManagementApplicationContractsModule : AbpModule
{

}
