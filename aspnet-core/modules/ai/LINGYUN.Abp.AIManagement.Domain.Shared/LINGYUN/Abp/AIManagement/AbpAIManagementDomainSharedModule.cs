using LINGYUN.Abp.AIManagement.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.AIManagement;

[DependsOn(typeof(AbpDddDomainSharedModule))]
public class AbpAIManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAIManagementDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Add<AIManagementResource>()
                .AddVirtualJson("/LINGYUN/Abp/AIManagement/Localization/Resources");
        });
    }
}
