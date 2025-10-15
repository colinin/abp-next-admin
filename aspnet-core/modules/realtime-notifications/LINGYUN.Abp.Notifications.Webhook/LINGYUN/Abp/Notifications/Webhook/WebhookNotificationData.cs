using LINGYUN.Abp.Webhooks;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications.Webhook;
public class WebhookNotificationData
{
    public string WebhookName { get; }
    public object Data { get; }
    public bool SendExactSameData { get; set; }
    public WebhookHeader Headers { get; set; }
    public WebhookNotificationData(string webhookName, object data)
    {
        WebhookName = Check.NotNullOrWhiteSpace(webhookName, nameof(webhookName));
        Data = Check.NotNull(data, nameof(data));
    }
}
