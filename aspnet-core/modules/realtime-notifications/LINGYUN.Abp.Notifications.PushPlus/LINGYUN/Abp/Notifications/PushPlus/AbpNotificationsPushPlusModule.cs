using LINGYUN.Abp.PushPlus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.PushPlus;

[DependsOn(
    typeof(AbpNotificationsModule),
    typeof(AbpPushPlusModule))]
public class AbpNotificationsPushPlusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<PushPlusNotificationPublishProvider>();
        });

        // 声明本渠道支持的载荷形态(注册期即校验存在性契约)
        Configure<AbpNotificationChannelDataOptions>(options =>
        {
            options.Add<PushPlusChannelData>();
        });
    }
}
