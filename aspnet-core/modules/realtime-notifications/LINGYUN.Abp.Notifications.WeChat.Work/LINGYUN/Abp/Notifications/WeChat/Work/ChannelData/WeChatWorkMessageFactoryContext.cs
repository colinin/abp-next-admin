using LINGYUN.Abp.WeChat.Work.Messages;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信消息构造上下文
/// </summary>
public class WeChatWorkMessageFactoryContext
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
    public WeChatWorkChannelData ChannelData { get; }

    /// <summary>
    /// 消息接收者
    /// </summary>
    public WeChatWorkNotificationContact Contact { get; }

    /// <summary>
    /// 消息公共选项
    /// </summary>
    public WeChatWorkMessageOptions Options { get; }

    public WeChatWorkMessageFactoryContext(
        NotificationInfo notification,
        NotificationStandardData standardData,
        WeChatWorkChannelData channelData,
        WeChatWorkNotificationContact contact,
        WeChatWorkMessageOptions options)
    {
        Notification = notification;
        StandardData = standardData;
        ChannelData = channelData;
        Contact = contact;
        Options = options;
    }
}
