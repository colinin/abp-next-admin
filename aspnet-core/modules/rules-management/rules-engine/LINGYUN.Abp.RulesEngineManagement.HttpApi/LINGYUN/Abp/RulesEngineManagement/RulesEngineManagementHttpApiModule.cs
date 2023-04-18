using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RulesEngineManagement;

[DependsOn(
    typeof(RulesEngineManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class RulesEngineManagementHttpApiModule : AbpModule
{

}
