using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Notifications.Extensions;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Notifications;

[DependsOn(
    typeof(AbpNotificationsModule),
    typeof(AbpElsaNextModule))]
public class AbpElsaNextNotificationsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseAbpNotifications();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextNotificationsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/LINGYUN/Abp/ElsaNext/Notifications/Localization/Resources");
        });
    }
}
