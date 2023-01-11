namespace LINGYUN.Abp.WebhooksManagement;
public class WebhookManagementOptions
{
    public bool IsDynamicWebhookStoreEnabled { get; set; }

    public WebhookManagementOptions()
    {
        IsDynamicWebhookStoreEnabled = true;
    }
}
