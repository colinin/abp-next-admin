using LINGYUN.Abp.WxPusher;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WxPusher;

[DependsOn(
    typeof(AbpNotificationsModule),
    typeof(AbpWxPusherModule))]
public class AbpNotificationsWxPusherModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<WxPusherNotificationPublishProvider>();
        });
    }
}
