namespace LINGYUN.Abp.Notifications;

public class AbpNotificationsManagementOptions
{
    public bool IsDynamicNotificationStoreEnabled { get; set; }
    public AbpNotificationsManagementOptions()
    {
        IsDynamicNotificationStoreEnabled = true;
    }
}
