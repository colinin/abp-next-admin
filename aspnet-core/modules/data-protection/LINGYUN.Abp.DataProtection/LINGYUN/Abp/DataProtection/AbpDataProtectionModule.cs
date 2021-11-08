using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.DataProtection
{
    [DependsOn(
        typeof(AbpThreadingModule))]
    public class AbpDataProtectionModule : AbpModule
    {
    }
}
