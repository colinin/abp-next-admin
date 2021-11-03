using LINGYUN.Platform;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.UI.Navigation.VueVbenAdmin
{
    [DependsOn(
        typeof(AbpUINavigationModule),
        typeof(PlatformDomainModule))]
    public class AbpUINavigationVueVbenAdminModule : AbpModule
    {
    }
}
