using LINGYUN.Abp.ProjectManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ProjectManagement
{
    [DependsOn(
        typeof(AbpValidationModule))]
    public class AbpProjectManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpProjectManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpProjectManagementResource>()
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/LINGYUN/Abp/ProjectManagement/Localization/Domain");
            });
        }
    }
}
