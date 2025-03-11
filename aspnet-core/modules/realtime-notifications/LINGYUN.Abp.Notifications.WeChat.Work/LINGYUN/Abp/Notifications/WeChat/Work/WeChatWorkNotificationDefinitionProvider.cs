using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

public class WeChatWorkNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var group = context.AddGroup(
            WeChatWorkNotificationNames.GroupName,
            L("Notification:WeChatWork"),
            L("Notification:WeChatWork"));

        group.AddNotification(
            WeChatWorkNotificationNames.TestNotification,
            L("Notification:WeChatWorkTest"),
            L("Notification:WeChatWorkTestDesc"),
            allowSubscriptionToClients: true)
            .WithProviders(WeChatWorkNotificationPublishProvider.ProviderName);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
