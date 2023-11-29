using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 上下游共享应用事件
/// </summary>
/// <remarks>
/// 本事件触发时机为：
/// 1. 上游企业把自建应用共享给下游企业使用
/// 2. 上游企业把下游企业从共享应用中移除
/// </remarks>
[EventName("share_chain_change")]
public class ShareChainChangeEvent : WeChatWorkEventMessage
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ShareChainChangeEvent>(this);
    }
}
