using LINGYUN.Abp.WxPusher.Messages;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.Notifications.WxPusher;

public class NotificationsWxPusherTestsDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var group = context.GetGroupOrNull(NotificationsTestsNames.GroupName);

        var nt1 = group.Notifications.FirstOrDefault(n => n.Name.Equals(NotificationsTestsNames.Test1));
        nt1.WithProviders(WxPusherNotificationPublishProvider.ProviderName)
           .WithContentType(MessageContentType.Text)
           .WithTopics(new List<int> { 7182 });

        var nt2 = group.Notifications.FirstOrDefault(n => n.Name.Equals(NotificationsTestsNames.Test2));
        nt2.WithProviders(WxPusherNotificationPublishProvider.ProviderName)
           .WithContentType(MessageContentType.Html)
           .WithTopics(new List<int> { 7182 });

        var nt3 = group.Notifications.FirstOrDefault(n => n.Name.Equals(NotificationsTestsNames.Test3));
        nt3.WithProviders(WxPusherNotificationPublishProvider.ProviderName)
           .WithContentType(MessageContentType.Markdown)
           .WithTopics(new List<int> { 7182 });
    }
}
