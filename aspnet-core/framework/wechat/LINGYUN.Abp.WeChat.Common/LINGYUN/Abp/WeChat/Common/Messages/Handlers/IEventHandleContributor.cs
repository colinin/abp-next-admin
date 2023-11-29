using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Common.Messages.Handlers;
public interface IEventHandleContributor<TMessage> where TMessage : WeChatEventMessage
{
    Task HandleAsync(MessageHandleContext<TMessage> context);
}
