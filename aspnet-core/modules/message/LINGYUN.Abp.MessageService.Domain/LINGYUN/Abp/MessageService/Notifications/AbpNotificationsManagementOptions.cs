namespace LINGYUN.Abp.MessageService.Notifications;

public class AbpNotificationsManagementOptions
{
    public bool IsDynamicNotificationStoreEnabled { get; set; }
    public AbpNotificationsManagementOptions()
    {
        IsDynamicNotificationStoreEnabled = true;
    }
}
