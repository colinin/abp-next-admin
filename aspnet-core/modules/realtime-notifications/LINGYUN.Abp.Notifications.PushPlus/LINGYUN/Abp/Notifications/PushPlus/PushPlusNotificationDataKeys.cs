namespace LINGYUN.Abp.Notifications.PushPlus;

/// <summary>
/// PushPlus 通知数据键
/// </summary>
public static class PushPlusNotificationDataKeys
{
    public const string Prefix = "push-plus:";

    /// <summary>
    /// webhook 地址
    /// </summary>
    public const string Webhook = Prefix + "webhook";
    /// <summary>
    /// 回调地址
    /// </summary>
    public const string CallbackUrl = Prefix + "callback";
}
