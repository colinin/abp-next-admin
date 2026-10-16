using LINGYUN.Abp.WeChat.Work.Messages;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信消息构造结果
/// </summary>
public class WeChatWorkMessageFactoryResult
{
    /// <summary>
    /// 消息载体
    /// </summary>
    public WeChatWorkMessage Message { get; }

    /// <summary>
    /// 消息载体类型, 用于日志与排障
    /// </summary>
    public WeChatWorkMessageKind Kind { get; }

    public WeChatWorkMessageFactoryResult(WeChatWorkMessage message, WeChatWorkMessageKind kind)
    {
        Message = message;
        Kind = kind;
    }

    public static WeChatWorkMessageFactoryResult Create(WeChatWorkMessage message, WeChatWorkMessageKind kind)
    {
        return new WeChatWorkMessageFactoryResult(message, kind);
    }
}
