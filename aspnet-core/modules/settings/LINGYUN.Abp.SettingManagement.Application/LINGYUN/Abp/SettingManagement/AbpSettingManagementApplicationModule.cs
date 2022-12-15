using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace LINGYUN.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpDddApplicationModule)
        )]
    public class AbpSettingManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddScoped<ISettingTestAppService, SettingAppService>();
        }
    }
}
