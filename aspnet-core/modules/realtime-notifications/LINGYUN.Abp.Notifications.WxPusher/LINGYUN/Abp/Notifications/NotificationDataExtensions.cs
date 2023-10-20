namespace LINGYUN.Abp.Notifications;
public static class NotificationDataExtensions
{
    private const string Prefix = "wx-pusher:";
    private const string UrlKey = Prefix + "url";

    public static void SetUrl(
        this NotificationData notificationData,
        string url)
    {
        notificationData.TrySetData(UrlKey, url);
    }

    public static string GetUrlOrNull(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(UrlKey)?.ToString();
    }
}
