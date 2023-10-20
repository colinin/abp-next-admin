namespace LINGYUN.Abp.PushPlus.Message;

public enum PushPlusMessageStatus
{
    /// <summary>
    /// 未发送
    /// </summary>
    NotSend = 0,
    /// <summary>
    /// 发送中
    /// </summary>
    Sending = 1,
    /// <summary>
    /// 已发送
    /// </summary>
    Successed = 2,
    /// <summary>
    /// 发送失败
    /// </summary>
    Failed = 3,
}
