using LINGYUN.Abp.WeChat.Work;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

[DependsOn(
    typeof(AbpNotificationsWebhookModule),
    typeof(AbpWeChatWorkModule))]
public class AbpNotificationsWebhookWeChatWorkModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsWebhookWeChatWorkOptions>(options =>
        {
            options.UseMarkdownV2 = true;
        });

        Configure<AbpNotificationsWebhookOptions>(options =>
        {
            options.Contributors.Add(new WeChatWorkWebhookNotificationContributor());
        });
    }
}
