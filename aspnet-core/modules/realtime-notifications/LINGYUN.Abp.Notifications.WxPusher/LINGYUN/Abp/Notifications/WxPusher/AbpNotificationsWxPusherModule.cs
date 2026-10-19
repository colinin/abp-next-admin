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

        // 声明本渠道支持的载荷形态(注册期即校验存在性契约)
        Configure<AbpNotificationChannelDataOptions>(options =>
        {
            options.Add<WxPusherChannelData>();
        });
    }
}
