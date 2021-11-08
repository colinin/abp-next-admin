using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpDataProtectionModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class AbpDataProtectionEntityFrameworkCoreModule : AbpModule
    {
    }
}
