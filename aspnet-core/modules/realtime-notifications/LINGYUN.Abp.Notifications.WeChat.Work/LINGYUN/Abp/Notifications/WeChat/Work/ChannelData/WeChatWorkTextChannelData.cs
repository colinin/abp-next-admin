namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 文本消息载荷
/// </summary>
/// <remarks>
/// 不参与渠道解析: 文本消息由通知标准数据(title/message)推导,
/// 生产者不需要为它准备任何渠道专属数据。
/// </remarks>
public class WeChatWorkTextChannelData : WeChatWorkChannelData
{
    public override WeChatWorkMessageKind Kind => WeChatWorkMessageKind.Text;

    /// <summary>
    /// 消息内容, 为空时使用通知标准数据
    /// </summary>
    public virtual string? Content { get; set; }
}
