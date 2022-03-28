using Volo.Abp.Domain;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Webhooks.Identity;

[DependsOn(typeof(AbpDddDomainModule))]
[DependsOn(typeof(AbpEventBusModule))]
[DependsOn(typeof(AbpUsersAbstractionModule))]
[DependsOn(typeof(AbpIdentityDomainSharedModule))]
public class AbpWebhooksIdentityModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpWebhooksIdentityModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityResource>()
                .AddVirtualJson("/LINGYUN/Abp/Webhooks/Identity/Localization/Resources");
        });
    }
}
