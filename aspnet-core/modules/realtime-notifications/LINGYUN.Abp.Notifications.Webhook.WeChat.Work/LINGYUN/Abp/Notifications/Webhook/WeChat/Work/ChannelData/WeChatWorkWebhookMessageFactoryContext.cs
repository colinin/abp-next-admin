using LINGYUN.Abp.WeChat.Work.Messages;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 消息构造上下文
/// </summary>
public class WeChatWorkWebhookMessageFactoryContext
{
    /// <summary>
    /// 通知消息
    /// </summary>
    public NotificationInfo Notification { get; }

    /// <summary>
    /// 通知标准数据(已完成本地化与模板渲染)
    /// </summary>
    public NotificationStandardData StandardData { get; }

    /// <summary>
    /// 渠道数据(载荷)
    /// </summary>
    public WeChatWorkWebhookChannelData ChannelData { get; }

    /// <summary>
    /// 渠道选项
    /// </summary>
    public AbpNotificationsWebhookWeChatWorkOptions Options { get; }

    public WeChatWorkWebhookMessageFactoryContext(
        NotificationInfo notification,
        NotificationStandardData standardData,
        WeChatWorkWebhookChannelData channelData,
        AbpNotificationsWebhookWeChatWorkOptions options)
    {
        Notification = notification;
        StandardData = standardData;
        ChannelData = channelData;
        Options = options;
    }
}
