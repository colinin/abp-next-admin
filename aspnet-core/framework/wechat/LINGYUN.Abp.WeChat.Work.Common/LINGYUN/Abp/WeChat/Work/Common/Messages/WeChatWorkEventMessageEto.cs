using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;

[GenericEventName(Prefix = "wechat.work.events")]
public class WeChatWorkEventMessageEto<TEvent> : WeChatMessageEto
    where TEvent : WeChatWorkEventMessage
{
    public TEvent Event { get; set; }
    public WeChatWorkEventMessageEto()
    {

    }
    public WeChatWorkEventMessageEto(TEvent @event)
    {
        Event = @event;
    }
}