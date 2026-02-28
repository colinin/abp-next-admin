using LINGYUN.Abp.WeChat.Common.Messages;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Official.Messages;
/// <summary>
/// 微信公众号事件处理器
/// </summary>
public class WeChatOfficialEventResolveContributor : WeChatOfficialMessageResolveContributorBase
{
    public override string Name => "WeChat.Official.Event";

    protected override Task ResolveMessageAsync(IMessageResolveContext context, AbpWeChatOfficialMessageResolveOptions options)
    {
        var messageType = context.GetMessageData("MsgType");
        var eventName = context.GetMessageData("Event");
        if ("event".Equals(messageType, StringComparison.InvariantCultureIgnoreCase) && 
            !eventName.IsNullOrWhiteSpace() && 
            options.EventMaps.TryGetValue(eventName, out var eventFactory))
        {
            context.Message = eventFactory(context);
            context.Handled = true;
        }
        return Task.CompletedTask;
    }
}
