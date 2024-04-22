using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 弹出微信相册发图器的事件推送
/// </summary>
[EventName("pic_weixin")]
public class PictureWeixinPushEvent : PicturePushEvent
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<PictureWeixinPushEvent>(this);
    }
}