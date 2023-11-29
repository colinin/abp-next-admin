using LINGYUN.Abp.WeChat.Common.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;
/// <summary>
/// 微信公众号事件处理器
/// </summary>
public class WeChatWorkEventResolveContributor : WeChatWorkMessageResolveContributorBase
{
    public override string Name => "WeChat.Work.Event";

    protected override Task ResolveWeComMessageAsync(IMessageResolveContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpWeChatWorkMessageResolveOptions>>().Value;
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
