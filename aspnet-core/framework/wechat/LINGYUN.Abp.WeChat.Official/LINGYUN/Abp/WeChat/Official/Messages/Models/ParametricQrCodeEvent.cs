using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Official.Messages.Models;
/// <summary>
/// 扫描带参数二维码事件
/// </summary>
[EventName("parametric_qr_code")]
public class ParametricQrCodeEvent : WeChatEventMessage
{
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
    /// <summary>
    /// 二维码的ticket，可用来换取二维码图片
    /// </summary>
    [XmlElement("Ticket")]
    public string Ticket { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatOfficialEventMessageEto<ParametricQrCodeEvent>(this);
    }
}
