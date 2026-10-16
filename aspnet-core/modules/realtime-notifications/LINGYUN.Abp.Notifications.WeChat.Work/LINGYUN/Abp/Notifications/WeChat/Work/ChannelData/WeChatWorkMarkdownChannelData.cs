namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// markdown消息载荷
/// </summary>
/// <remarks>
/// 不参与渠道解析: 由通知定义的 <c>ContentType</c> 推导,
/// 生产者不需要为它准备任何渠道专属数据。
/// </remarks>
public class WeChatWorkMarkdownChannelData : WeChatWorkChannelData
{
    public override WeChatWorkMessageKind Kind => WeChatWorkMessageKind.Markdown;

    /// <summary>
    /// 消息内容, 为空时使用通知标准数据
    /// </summary>
    public virtual string? Content { get; set; }
}
