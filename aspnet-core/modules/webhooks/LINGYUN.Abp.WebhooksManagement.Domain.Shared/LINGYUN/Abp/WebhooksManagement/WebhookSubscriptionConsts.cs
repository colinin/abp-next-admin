namespace LINGYUN.Abp.WebhooksManagement;

public static class WebhookSubscriptionConsts
{
    public static int MaxWebhookUriLength { get; set; } = 255;
    public static int MaxSecretLength { get; set; } = 128;
    public static int MaxWebhooksLength { get; set; } = int.MaxValue;
    public static int MaxHeadersLength { get; set; } = int.MaxValue;
}
