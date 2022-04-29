namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 内置通知提供者
/// </summary>
public static class NotificationProviderNames
{
    /// <summary>
    /// SignalR 实时通知
    /// </summary>
    public const string SignalR = "SignalR";
    /// <summary>
    /// 短信通知
    /// </summary>
    public const string Sms = "Sms";
    /// <summary>
    /// 邮件通知
    /// </summary>
    public const string Emailing = "Emailing";
    /// <summary>
    /// 微信小程序模板通知
    /// </summary>
    public const string WechatMiniProgram = "WeChat.MiniProgram";
}
