using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtection;

[DependsOn(
    typeof(AbpDataProtectionAbstractionsModule),
    typeof(AbpDddApplicationContractsModule))]
public class AbpDataProtectionApplicationContractsModule : AbpModule
{

}
