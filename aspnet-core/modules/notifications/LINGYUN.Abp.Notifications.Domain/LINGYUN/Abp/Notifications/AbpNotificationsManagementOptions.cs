namespace LINGYUN.Abp.Notifications;

public class AbpNotificationsManagementOptions
{
    public bool SaveStaticNotificationsToDatabase { get; set; }
    public bool IsDynamicNotificationsStoreEnabled { get; set; }
    public AbpNotificationsManagementOptions()
    {
        SaveStaticNotificationsToDatabase = true;
        IsDynamicNotificationsStoreEnabled = true;
    }
}
