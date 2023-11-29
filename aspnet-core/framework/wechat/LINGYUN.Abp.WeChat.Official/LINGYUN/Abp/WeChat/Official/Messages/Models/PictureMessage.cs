using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Official.Messages.Models;
/// <summary>
/// 图片消息
/// </summary>
[EventName("picture")]
public class PictureMessage : WeChatOfficialGeneralMessage
{
    /// <summary>
    /// 图片链接（由系统生成）
    /// </summary>
    [XmlElement("PicUrl")]
    public string PicUrl { get; set; }
    /// <summary>
    /// 图片消息媒体id，可以调用获取临时素材接口拉取数据。
    /// </summary>
    [XmlElement("MediaId")]
    public string MediaId { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatOfficialGeneralMessageEto<PictureMessage>(this);
    }
}
