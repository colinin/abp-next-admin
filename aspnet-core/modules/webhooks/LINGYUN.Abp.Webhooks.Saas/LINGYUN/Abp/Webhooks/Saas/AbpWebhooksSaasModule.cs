using LINGYUN.Abp.Saas;
using LINGYUN.Abp.Saas.Localization;
using Volo.Abp.Domain;
using Volo.Abp.EventBus;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Webhooks.Saas;

[DependsOn(typeof(AbpDddDomainModule))]
[DependsOn(typeof(AbpEventBusModule))]
[DependsOn(typeof(AbpSaasDomainSharedModule))]
public class AbpWebhooksSaasModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpWebhooksSaasModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpSaasResource>()
                .AddVirtualJson("/LINGYUN/Abp/Webhooks/Saas/Localization/Resources");
        });
    }
}
