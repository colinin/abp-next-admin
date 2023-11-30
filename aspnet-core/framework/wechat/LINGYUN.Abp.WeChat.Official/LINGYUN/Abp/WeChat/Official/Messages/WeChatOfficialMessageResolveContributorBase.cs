using LINGYUN.Abp.WeChat.Common.Messages;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Official.Messages;
public abstract class WeChatOfficialMessageResolveContributorBase : MessageResolveContributorBase
{
    public override Task ResolveAsync(IMessageResolveContext context)
    {
        if (!context.HasMessageKey("AgentID"))
        {
            return ResolveWeChatMessageAsync(context);
        }

        return Task.CompletedTask;
    }

    protected abstract Task ResolveWeChatMessageAsync(IMessageResolveContext context);
}
