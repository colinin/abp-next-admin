using LINGYUN.Abp.WeChat.Common.Messages;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Common.Messages.Handlers;
public interface IMessageHandleContributor<TMessage> where TMessage : WeChatGeneralMessage
{
    Task HandleAsync(MessageHandleContext<TMessage> context);
}
