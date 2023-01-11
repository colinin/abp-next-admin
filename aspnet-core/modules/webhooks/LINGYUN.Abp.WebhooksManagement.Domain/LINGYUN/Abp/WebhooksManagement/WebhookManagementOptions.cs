namespace LINGYUN.Abp.WebhooksManagement;
public class WebhookManagementOptions
{
    public bool SaveStaticWebhooksToDatabase { get; set; }
    public bool IsDynamicWebhookStoreEnabled { get; set; }

    public WebhookManagementOptions()
    {
        IsDynamicWebhookStoreEnabled = true;
        SaveStaticWebhooksToDatabase = true;
    }
}
