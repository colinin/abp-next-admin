using LINGYUN.Abp.WeChat.Common.Messages;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;
public abstract class WeChatWorkMessageResolveContributorBase : MessageResolveContributorBase
{
    public override Task ResolveAsync(IMessageResolveContext context)
    {
        // 企业微信消息需要企业标识字段
        if (context.HasMessageKey("AgentID"))
        {
            return ResolveWeComMessageAsync(context);
        }

        return Task.CompletedTask;
    }

    protected abstract Task ResolveWeComMessageAsync(IMessageResolveContext context);
}
