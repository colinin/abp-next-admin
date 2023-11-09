using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 弹出拍照或者相册发图的事件推送
/// </summary>
[EventName("pic_photo_or_album")]
public class PictureAlbumPushEvent : PicturePushEvent
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<PictureAlbumPushEvent>(this);
    }
}