using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;

[GenericEventName(Prefix = "wechat.work.messages")]
public class WeChatWorkGeneralMessageEto<TMessage> : WeChatMessageEto
    where TMessage : WeChatWorkGeneralMessage
{
    public TMessage Message { get; set; }
    public WeChatWorkGeneralMessageEto()
    {

    }
    public WeChatWorkGeneralMessageEto(TMessage message)
    {
        Message = message;
    }
}
