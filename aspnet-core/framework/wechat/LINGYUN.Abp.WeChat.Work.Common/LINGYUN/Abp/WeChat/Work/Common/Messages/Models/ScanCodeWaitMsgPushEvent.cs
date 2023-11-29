using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 扫码推事件且弹出“消息接收中”提示框的事件推送
/// </summary>
[EventName("scancode_waitmsg")]
public class ScanCodeWaitMsgPushEvent : ScanCodeEvent
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ScanCodeWaitMsgPushEvent>(this);
    }
}
