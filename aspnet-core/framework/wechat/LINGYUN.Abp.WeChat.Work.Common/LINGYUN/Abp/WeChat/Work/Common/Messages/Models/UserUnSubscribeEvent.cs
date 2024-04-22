using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 用户取消关注事件
/// </summary>
[EventName("user_un_subscribe")]
public class UserUnSubscribeEvent : WeChatWorkEventMessage
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<UserUnSubscribeEvent>(this);
    }
}
