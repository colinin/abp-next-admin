using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Messages
{
    public interface IMessageSenderProvider
    {
        string Name { get; }
        Task SendMessageAsync(ChatMessage chatMessage);
    }
}
