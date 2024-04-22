using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Official.Messages;

[GenericEventName(Prefix = "wechat.official.messages")]
public class WeChatOfficialGeneralMessageEto<TMessage> : WeChatMessageEto
    where TMessage : WeChatOfficialGeneralMessage
{
    public TMessage Message { get; set; }
    public WeChatOfficialGeneralMessageEto()
    {

    }
    public WeChatOfficialGeneralMessageEto(TMessage message)
    {
        Message = message;
    }
}
