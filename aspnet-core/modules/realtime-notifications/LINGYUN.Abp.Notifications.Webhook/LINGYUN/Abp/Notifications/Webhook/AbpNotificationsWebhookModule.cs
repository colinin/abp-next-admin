using LINGYUN.Abp.Webhooks;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Webhook;

[DependsOn(
    typeof(AbpNotificationsCoreModule),
    typeof(AbpWebhooksModule))]
public class AbpNotificationsWebhookModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<WebhookNotificationPublishProvider>();
        });
    }
}
