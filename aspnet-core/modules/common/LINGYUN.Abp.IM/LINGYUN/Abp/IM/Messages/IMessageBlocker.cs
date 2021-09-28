using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Messages
{
    public interface IMessageBlocker
    {
        Task InterceptAsync(ChatMessage message);
    }
}
