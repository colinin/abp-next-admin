using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Webhooks.Extensions;
using LINGYUN.Abp.Webhooks;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Webhooks;

[DependsOn(
    typeof(AbpWebhooksModule),
    typeof(AbpElsaNextModule))]
public class AbpElsaNextWebhooksModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseAbpWebhooks();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextWebhooksModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/LINGYUN/Abp/ElsaNext/Webhooks/Localization/Resources");
        });
    }
}
