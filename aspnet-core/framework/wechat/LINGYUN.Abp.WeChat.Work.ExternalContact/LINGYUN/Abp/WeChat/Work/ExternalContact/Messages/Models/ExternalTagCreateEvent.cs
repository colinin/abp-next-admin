using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 企业客户标签创建事件推送
/// </summary>
[EventName("change_external_tag_create")]
public class ExternalTagCreateEvent : ExternalTagChangeEvent
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ExternalTagCreateEvent>(this);
    }
}
