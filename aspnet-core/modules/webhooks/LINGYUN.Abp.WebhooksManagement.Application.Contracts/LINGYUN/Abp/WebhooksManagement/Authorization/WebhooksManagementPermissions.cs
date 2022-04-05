namespace LINGYUN.Abp.WebhooksManagement.Authorization;

public static class WebhooksManagementPermissions
{
    public const string GroupName = "AbpWebhooks";

    /// <summary>
    /// 授权允许发布Webhooks事件, 建议客户端授权
    /// </summary>
    public const string Publish = GroupName + ".Publish";

    public const string ManageSettings = GroupName + ".ManageSettings";

    public static class WebhookSubscription
    {
        public const string Default = GroupName + ".Subscriptions";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class WebhooksSendAttempts
    {
        public const string Default = GroupName + ".SendAttempts";
        public const string Delete = Default + ".Delete";
        public const string Resend = Default + ".Resend";
    }
}
