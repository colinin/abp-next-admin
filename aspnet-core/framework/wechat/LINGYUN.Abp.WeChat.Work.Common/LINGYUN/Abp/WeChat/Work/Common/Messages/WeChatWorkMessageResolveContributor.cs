using LINGYUN.Abp.WeChat.Common.Messages;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;
/// <summary>
/// 企业微信消息处理器
/// </summary>
public class WeChatWorkMessageResolveContributor : WeChatWorkMessageResolveContributorBase
{
    public override string Name => "WeChat.Work.Message";

    protected override Task ResolveMessageAsync(IMessageResolveContext context, AbpWeChatWorkMessageResolveOptions options)
    {
        var messageType = context.GetMessageData("MsgType");
        if (options.MessageMaps.TryGetValue(messageType, out var messageFactory))
        {
            context.Message = messageFactory(context);
            context.Handled = true;
        }
        return Task.CompletedTask;
    }
}
