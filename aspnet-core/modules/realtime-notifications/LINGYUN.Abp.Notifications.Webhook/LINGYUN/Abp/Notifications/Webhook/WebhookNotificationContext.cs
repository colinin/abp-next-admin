using System;

namespace LINGYUN.Abp.Notifications.Webhook;
public class WebhookNotificationContext : IWebhookNotificationContext
{
    public IServiceProvider ServiceProvider { get; }
    public NotificationInfo Notification { get; }
    public WebhookNotificationData Webhook { get; set; }
    public bool Handled { get; set; }
    public WebhookNotificationContext(IServiceProvider serviceProvider, NotificationInfo notification)
    {
        ServiceProvider = serviceProvider;
        Notification = notification;
    }
    public bool HasResolved()
    {
        return Handled || Webhook != null;
    }
}
