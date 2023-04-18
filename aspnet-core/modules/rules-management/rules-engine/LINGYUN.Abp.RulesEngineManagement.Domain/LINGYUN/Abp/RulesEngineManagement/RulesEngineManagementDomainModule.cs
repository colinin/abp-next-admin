using LINGYUN.Abp.Rules.RulesEngine;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RulesEngineManagement;

[DependsOn(
    typeof(RulesEngineManagementDomainSharedModule),
    typeof(AbpRulesEngineModule),
    typeof(AbpDddDomainModule))]
public class RulesEngineManagementDomainModule : AbpModule
{

}
