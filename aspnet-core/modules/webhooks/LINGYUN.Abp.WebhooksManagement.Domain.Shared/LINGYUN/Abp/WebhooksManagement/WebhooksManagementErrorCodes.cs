namespace LINGYUN.Abp.WebhooksManagement;

public static class WebhooksManagementErrorCodes
{
    public const string Namespace = "Webhooks";

    
    public static class WebhookGroupDefinition
    {
        public const string Prefix = Namespace + ":001";

        public const string AlreayNameExists = Prefix + "001";

        public const string StaticGroupNotAllowedChanged = Prefix + "010";
    }

    public static class WebhookDefinition
    {
        public const string Prefix = Namespace + ":002";

        public const string AlreayNameExists = Prefix + "001";

        public const string StaticWebhookNotAllowedChanged = Prefix + "010";
    }

    public static class WebhookSubscription
    {
        public const string Prefix = Namespace + ":010";

        public const string DuplicateSubscribed = Prefix + "001";
    }
}
