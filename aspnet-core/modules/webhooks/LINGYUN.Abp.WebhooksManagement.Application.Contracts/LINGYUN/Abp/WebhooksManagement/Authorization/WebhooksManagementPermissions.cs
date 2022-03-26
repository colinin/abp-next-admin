namespace LINGYUN.Abp.WebhooksManagement.Authorization;

public static class WebhooksManagementPermissions
{
    public const string GroupName = "WebhooksManagement";

    /// <summary>
    /// 授权允许发布Webhooks事件, 建议客户端授权
    /// </summary>
    public const string Publish = GroupName + ".Publish";

    public const string ManageSettings = GroupName + ".ManageSettings";
}
