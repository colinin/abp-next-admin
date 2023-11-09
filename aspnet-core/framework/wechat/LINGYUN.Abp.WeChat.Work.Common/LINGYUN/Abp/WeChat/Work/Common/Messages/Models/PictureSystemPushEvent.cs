using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 弹出系统拍照发图的事件推送
/// </summary>
[EventName("pic_sysphoto")]
public class PictureSystemPushEvent : PicturePushEvent
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<PictureSystemPushEvent>(this);
    }
}
