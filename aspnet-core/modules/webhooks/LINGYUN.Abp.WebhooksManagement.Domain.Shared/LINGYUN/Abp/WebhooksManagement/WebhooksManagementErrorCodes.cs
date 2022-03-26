namespace LINGYUN.Abp.WebhooksManagement;

public static class WebhooksManagementErrorCodes
{
    public const string Namespace = "Webhooks";

    public static class WebhookSubscription
    {
        public const string Prefix = Namespace + ":010";

        public const string DuplicateSubscribed = Prefix + "001";
    }
}
