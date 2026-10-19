namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 渠道数据(载荷)基类
/// </summary>
/// <remarks>
/// 每个载荷对应一种 Webhook 消息载体, 通过 <see cref="Kind"/> 显式声明形态,
/// 渠道侧使用工厂字典做 O(1) 分派。
/// </remarks>
public abstract class WeChatWorkWebhookChannelData : NotificationChannelData
{
    /// <summary>
    /// 消息载体类型
    /// </summary>
    public abstract WeChatWorkWebhookMessageKind Kind { get; }
}
