using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications.Webhook;
public class AbpNotificationsWebhookOptions
{
    public IList<IWebhookNotificationContributor> Contributors { get; }
    public AbpNotificationsWebhookOptions()
    {
        Contributors = new List<IWebhookNotificationContributor>();
    }
}
