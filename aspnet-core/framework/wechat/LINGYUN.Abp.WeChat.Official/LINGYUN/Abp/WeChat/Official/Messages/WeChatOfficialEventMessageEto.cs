using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Official.Messages;

[GenericEventName(Prefix = "wechat.official.events")]
public class WeChatOfficialEventMessageEto<TEvent> : WeChatMessageEto
    where TEvent : WeChatEventMessage
{
    public TEvent Event { get; set; }
    public WeChatOfficialEventMessageEto()
    {

    }
    public WeChatOfficialEventMessageEto(TEvent @event)
    {
        Event = @event;
    }
}
