using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Common.Messages;
public interface IMessageResolver
{
    Task<MessageResolveResult> ResolveMessageAsync(MessageResolveData messageData);
}
