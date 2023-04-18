using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RulesEngineManagement;

[DependsOn(
    typeof(RulesEngineManagementApplicationContractsModule),
    typeof(RulesEngineManagementDomainModule),
    typeof(AbpDddApplicationModule))]
public class RulesEngineManagementApplicationModule : AbpModule
{

}
