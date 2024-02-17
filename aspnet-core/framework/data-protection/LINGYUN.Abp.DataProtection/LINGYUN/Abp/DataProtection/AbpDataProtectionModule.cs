using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtection;

[DependsOn(
    typeof(AbpDddDomainModule))]
public class AbpDataProtectionModule : AbpModule
{
}
