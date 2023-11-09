using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Official.Messages.Models;
/// <summary>
/// 语音消息
/// </summary>
[EventName("voice")]
public class VoiceMessage : WeChatOfficialGeneralMessage
{
    /// <summary>
    /// 语音格式，如amr，speex等
    /// </summary>
    [XmlElement("Format")]
    public string Format { get; set; }
    /// <summary>
    /// 语音识别结果，UTF8编码
    /// </summary>
    /// <remarks>
    /// 开通语音识别后，用户每次发送语音给公众号时，微信会在推送的语音消息XML数据包中，增加一个Recognition字段（
    /// 注：由于客户端缓存，开发者开启或者关闭语音识别功能，对新关注者立刻生效，对已关注用户需要24小时生效。开发者可以重新关注此账号进行测试）。
    /// </remarks>
    [XmlElement("Recognition")]
    public string Recognition { get; set; }
    /// <summary>
    /// 语音消息媒体id，可以调用获取临时素材接口拉取该媒体
    /// </summary>
    [XmlElement("MediaId")]
    public string MediaId { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatOfficialGeneralMessageEto<VoiceMessage>(this);
    }
}
