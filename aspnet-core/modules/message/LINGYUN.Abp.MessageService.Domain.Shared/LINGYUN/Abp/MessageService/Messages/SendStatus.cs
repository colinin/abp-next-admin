namespace LINGYUN.Abp.MessageService.Messages
{
    /// <summary>
    /// 消息状态
    /// </summary>
    public enum SendStatus : sbyte
    {
        /// <summary>
        /// 已发送
        /// </summary>
        Send = 0,
        /// <summary>
        /// 撤回
        /// </summary>
        ReCall = 10,
        /// <summary>
        /// 发送失败
        /// </summary>
        Failed = 50,
        /// <summary>
        /// 退回
        /// </summary>
        BackTo = 100
    }
}
