using LINGYUN.Abp.WebhooksManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WebhooksManagement;

[DependsOn(
    typeof(AbpLocalizationModule))]
public class WebhooksManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WebhooksManagementDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<WebhooksManagementResource>()
                .AddVirtualJson("/LINGYUN/Abp/WebhooksManagement/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(WebhooksManagementErrorCodes.Namespace, typeof(WebhooksManagementResource));
        });
    }
}
