using LINGYUN.Abp.Dingtalk;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Dingtalk;

[DependsOn(
    typeof(AbpNotificationsCoreModule),
    typeof(AbpDingtalkModule))]
public class AbpNotificationsDingtalkModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<DingtalkNotificationPublishProvider>();
        });
    }
}
