using LINGYUN.Abp.RulesManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.RulesManagement
{
    [DependsOn(
        typeof(AbpValidationModule))]
    public class AbpRulesManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpRulesManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<RulesResource>()
                    .AddVirtualJson("/LINGYUN/Abp/RulesManagement/Localization/Resources");
            });
        }
    }
}
