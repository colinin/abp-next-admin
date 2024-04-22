using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 长期未使用应用重新启用事件
/// </summary>
[EventName("reopen_inactive_agent")]
public class ReOpenInActiveAgentEevent : WeChatWorkEventMessage
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ReOpenInActiveAgentEevent>(this);
    }
}