namespace LINGYUN.Abp.Notifications.WxPusher;

/// <summary>
/// WxPusher 通知数据键
/// </summary>
public static class WxPusherNotificationDataKeys
{
    public const string Prefix = "wx-pusher:";

    /// <summary>
    /// 消息点击后的跳转地址
    /// </summary>
    public const string Url = Prefix + "url";
}
