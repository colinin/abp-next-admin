using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

[DependsOn(
    typeof(AbpWeChatWorkModule),
    typeof(AbpNotificationsModule))]
public class AbpNotificationsWeChatWorkModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpNotificationsWeChatWorkModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WeChatWorkResource>()
                .AddVirtualJson("/LINGYUN/Abp/Notifications/WeChat/Work/Localization/Resources");
        });

        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<WeChatWorkNotificationPublishProvider>();
        });
    }
}
