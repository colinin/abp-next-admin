using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 语音消息
/// </summary>
[EventName("voice")]
public class VoiceMessage : WeChatWorkGeneralMessage
{
    /// <summary>
    /// 语音格式，如amr，speex等
    /// </summary>
    [XmlElement("Format")]
    public string Format { get; set; }
    /// <summary>
    /// 语音消息媒体id，可以调用获取临时素材接口拉取该媒体
    /// </summary>
    [XmlElement("MediaId")]
    public string MediaId { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkGeneralMessageEto<VoiceMessage>(this);
    }
}
