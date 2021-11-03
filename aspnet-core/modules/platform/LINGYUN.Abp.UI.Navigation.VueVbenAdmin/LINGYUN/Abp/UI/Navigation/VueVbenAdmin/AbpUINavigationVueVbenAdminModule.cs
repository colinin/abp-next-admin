using LINGYUN.Platform;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.UI.Navigation.VueVbenAdmin
{
    [DependsOn(
        typeof(AbpUINavigationModule),
        typeof(PlatformDomainModule))]
    public class AbpUINavigationVueVbenAdminModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.NavigationSeedContributors.Add<VueVbenAdminNavigationSeedContributor>();
            });
        }
    }
}
