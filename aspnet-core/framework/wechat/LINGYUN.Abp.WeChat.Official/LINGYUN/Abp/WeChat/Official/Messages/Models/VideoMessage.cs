using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Official.Messages.Models;
/// <summary>
/// 视频消息
/// </summary>
[EventName("video")]
public class VideoMessage : WeChatOfficialGeneralMessage
{
    /// <summary>
    /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
    /// </summary>
    [XmlElement("ThumbMediaId")]
    public string ThumbMediaId { get; set; }
    /// <summary>
    /// 视频消息媒体id，可以调用获取临时素材接口拉取数据。
    /// </summary>
    [XmlElement("MediaId")]
    public string MediaId { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatOfficialGeneralMessageEto<VideoMessage>(this);
    }
}
