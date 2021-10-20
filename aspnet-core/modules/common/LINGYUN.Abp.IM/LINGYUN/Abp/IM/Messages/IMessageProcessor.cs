using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Messages
{
    /// <summary>
    /// 消息处理器
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReCallAsync(ChatMessage message);
        /// <summary>
        /// 消息已读
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReadAsync(ChatMessage message);
    }
}
