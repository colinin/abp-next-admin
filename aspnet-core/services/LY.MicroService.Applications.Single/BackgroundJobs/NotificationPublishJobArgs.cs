using LINGYUN.Abp.Notifications;

namespace LY.MicroService.Applications.Single.BackgroundJobs;

public class NotificationPublishJobArgs
{
    public Guid? TenantId { get; set; }
    public long NotificationId { get; set; }
    public string ProviderType { get; set; }
    public List<UserIdentifier> UserIdentifiers { get; set; }
    public NotificationPublishJobArgs()
    {
        UserIdentifiers = new List<UserIdentifier>();
    }
    public NotificationPublishJobArgs(long id, string providerType, List<UserIdentifier> userIdentifiers, Guid? tenantId = null)
    {
        NotificationId = id;
        ProviderType = providerType;
        UserIdentifiers = userIdentifiers;
        TenantId = tenantId;
    }
}
