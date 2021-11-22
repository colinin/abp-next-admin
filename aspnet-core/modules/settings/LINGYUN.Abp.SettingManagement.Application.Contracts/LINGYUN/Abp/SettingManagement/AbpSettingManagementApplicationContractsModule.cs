using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.SettingManagement
{
    [DependsOn(typeof(AbpDddApplicationContractsModule))]
    [DependsOn(typeof(AbpSettingManagementDomainSharedModule))]
    public class AbpSettingManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpSettingManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                .Get<AbpSettingManagementResource>()
                .AddVirtualJson("/LINGYUN/Abp/SettingManagement/Localization/ApplicationContracts");
            });
        }
    }
}
