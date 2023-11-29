using LINGYUN.Abp.WeChat.Common.Messages;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Common.Messages.Handlers;
public interface IMessageHandler
{
    Task HandleEventAsync<TMessage>(TMessage data) where TMessage : WeChatEventMessage;

    Task HandleMessageAsync<TMessage>(TMessage data) where TMessage : WeChatGeneralMessage;
}
