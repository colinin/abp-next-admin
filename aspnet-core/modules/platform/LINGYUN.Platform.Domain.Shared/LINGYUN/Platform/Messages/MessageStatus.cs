namespace LINGYUN.Platform.Messages;
public enum MessageStatus
{
    /// <summary>
    /// 未发送
    /// </summary>
    Pending = -1,
    /// <summary>
    /// 已发送
    /// </summary>
    Sent = 0,
    /// <summary>
    /// 发送失败
    /// </summary>
    Failed = 10
}
