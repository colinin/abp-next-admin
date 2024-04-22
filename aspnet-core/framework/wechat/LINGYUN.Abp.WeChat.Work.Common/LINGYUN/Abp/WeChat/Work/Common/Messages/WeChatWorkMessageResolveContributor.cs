using LINGYUN.Abp.WeChat.Common.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;
/// <summary>
/// 微信公众号消息处理器
/// </summary>
public class WeChatWorkMessageResolveContributor : WeChatWorkMessageResolveContributorBase
{
    public override string Name => "WeChat.Work.Message";

    protected override Task ResolveWeComMessageAsync(IMessageResolveContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpWeChatWorkMessageResolveOptions>>().Value;
        var messageType = context.GetMessageData("MsgType");
        if (options.MessageMaps.TryGetValue(messageType, out var messageFactory))
        {
            context.Message = messageFactory(context);
            context.Handled = true;
        }
        return Task.CompletedTask;
    }
}
