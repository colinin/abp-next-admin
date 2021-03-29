using LINGYUN.Abp.LocalizationManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.LocalizationManagement
{
    [DependsOn(
        typeof(AbpValidationModule),
        typeof(AbpLocalizationModule))]
    public class AbpLocalizationManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpLocalizationManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<LocalizationManagementResource>("en")
                    .AddVirtualJson("/LINGYUN/Abp/LocalizationManagement/Localization/Resources");
            });
        }
    }
}
