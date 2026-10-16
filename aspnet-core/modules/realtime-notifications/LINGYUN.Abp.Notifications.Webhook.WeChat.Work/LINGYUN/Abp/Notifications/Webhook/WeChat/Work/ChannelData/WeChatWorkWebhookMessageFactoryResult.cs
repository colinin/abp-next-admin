using LINGYUN.Abp.WeChat.Work.Messages;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 消息构造结果
/// </summary>
public class WeChatWorkWebhookMessageFactoryResult
{
    /// <summary>
    /// 消息载体
    /// </summary>
    public WeChatWorkWebhookMessage Message { get; }

    /// <summary>
    /// 消息载体类型, 用于日志与排障
    /// </summary>
    public WeChatWorkWebhookMessageKind Kind { get; }

    public WeChatWorkWebhookMessageFactoryResult(
        WeChatWorkWebhookMessage message,
        WeChatWorkWebhookMessageKind kind)
    {
        Message = message;
        Kind = kind;
    }

    public static WeChatWorkWebhookMessageFactoryResult Create(
        WeChatWorkWebhookMessage message,
        WeChatWorkWebhookMessageKind kind)
    {
        return new WeChatWorkWebhookMessageFactoryResult(message, kind);
    }
}
