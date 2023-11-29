using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 企业互联共享应用事件
/// </summary>
/// <remarks>
/// 本事件触发时机为：
/// 1. 上级企业把自建应用共享给下级企业使用
/// 2. 上级企业把下级企业从共享应用中移除
/// </remarks>
[EventName("share_agent_change")]
public class ShareAgentChangeEvent : WeChatWorkEventMessage
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ShareAgentChangeEvent>(this);
    }
}
