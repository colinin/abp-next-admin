using System.Linq;

namespace LINGYUN.Abp.Notifications.TuiJuhe;

public class NotificationsWxPusherTestsDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var group = context.GetGroupOrNull(NotificationsTestsNames.GroupName);

        var nt1 = group.Notifications.FirstOrDefault(n => n.Name.Equals(NotificationsTestsNames.Test1));
        nt1.WithProviders(TuiJuheNotificationPublishProvider.ProviderName)
           .WithServiceId("1d5v5GuH");

        var nt2 = group.Notifications.FirstOrDefault(n => n.Name.Equals(NotificationsTestsNames.Test2));
        nt2.WithProviders(TuiJuheNotificationPublishProvider.ProviderName)
           .WithServiceId("1d5v5GuH");

        var nt3 = group.Notifications.FirstOrDefault(n => n.Name.Equals(NotificationsTestsNames.Test3));
        nt3.WithProviders(TuiJuheNotificationPublishProvider.ProviderName)
           .WithServiceId("1d5v5GuH");

        var nt4 = group.Notifications.FirstOrDefault(n => n.Name.Equals(NotificationsTestsNames.Test4));
        nt4.WithProviders(TuiJuheNotificationPublishProvider.ProviderName)
           .WithServiceId("1d5v5GuH");
    }
}
