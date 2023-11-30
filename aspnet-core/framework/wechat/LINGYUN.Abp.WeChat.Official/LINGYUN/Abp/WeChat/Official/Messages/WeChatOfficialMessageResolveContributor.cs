using LINGYUN.Abp.WeChat.Common.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Official.Messages;
/// <summary>
/// 微信公众号消息处理器
/// </summary>
public class WeChatOfficialMessageResolveContributor : WeChatOfficialMessageResolveContributorBase
{
    public override string Name => "WeChat.Official.Message";

    protected override Task ResolveWeChatMessageAsync(IMessageResolveContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpWeChatOfficialMessageResolveOptions>>().Value;
        var messageType = context.GetMessageData("MsgType");
        if (options.MessageMaps.TryGetValue(messageType, out var messageFactory))
        {
            context.Message = messageFactory(context);
            context.Handled = true;
        }
        return Task.CompletedTask;
    }
}
