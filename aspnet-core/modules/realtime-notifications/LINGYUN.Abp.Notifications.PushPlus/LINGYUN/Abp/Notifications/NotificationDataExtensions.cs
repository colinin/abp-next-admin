namespace LINGYUN.Abp.Notifications;
public static class NotificationDataExtensions
{
    private const string Prefix = "push-plus:";
    private const string WebhookKey = Prefix + "webhook";
    private const string CallbackUrlKey = Prefix + "callback";

    public static void SetWebhook(
        this NotificationData notificationData,
        string url)
    {
        notificationData.TrySetData(WebhookKey, url);
    }

    public static string GetWebhookOrNull(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(WebhookKey)?.ToString();
    }

    public static void SetCallbackUrl(
       this NotificationData notificationData,
       string callbackUrl)
    {
        notificationData.TrySetData(CallbackUrlKey, callbackUrl);
    }

    public static string GetCallbackUrlOrNull(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(CallbackUrlKey)?.ToString();
    }
}
