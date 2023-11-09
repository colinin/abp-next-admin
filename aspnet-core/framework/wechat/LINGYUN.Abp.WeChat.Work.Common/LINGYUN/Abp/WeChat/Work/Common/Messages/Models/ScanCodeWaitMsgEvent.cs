using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 自定义菜单事件
/// </summary>
[EventName("scancode_waitmsg")]
public class ScanCodeWaitMsgEvent : ScanCodeEvent
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ScanCodeWaitMsgEvent>(this);
    }
}
