using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Messages
{
    /// <summary>
    /// 消息拦截器
    /// </summary>
    public interface IMessageBlocker
    {
        Task InterceptAsync(ChatMessage message);
    }
}
