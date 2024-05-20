using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.DataProtection;

[DependsOn(typeof(AbpObjectExtendingModule))]
public class AbpDataProtectionAbstractionsModule : AbpModule
{
}
