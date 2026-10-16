using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

[DependsOn(
    typeof(AbpNotificationsWebhookWeChatWorkModule),
    typeof(AbpNotificationsTestsModule),
    typeof(AbpTestsBaseModule))]
public class AbpNotificationsWebhookWeChatWorkTestsModule : AbpModule
{
}
