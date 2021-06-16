using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Messages
{
    public interface IMessageSender
    {
        Task<string> SendMessageAsync(ChatMessage chatMessage);
    }
}
