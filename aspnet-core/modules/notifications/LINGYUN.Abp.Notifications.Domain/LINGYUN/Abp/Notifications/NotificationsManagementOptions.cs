namespace LINGYUN.Abp.Notifications;

public class NotificationsManagementOptions
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticNotificationsToDatabase { get; set; } = true;

    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicNotificationsStoreEnabled { get; set; }

    public NotificationsManagementOptions()
    {
    }
}
