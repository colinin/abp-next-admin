using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtection;

[DependsOn(
    typeof(AbpDataProtectionApplicationContractsModule),
    typeof(AbpDataProtectionModule),
    typeof(AbpDddApplicationModule))]
public class AbpDataProtectionApplicationModule : AbpModule
{

}
