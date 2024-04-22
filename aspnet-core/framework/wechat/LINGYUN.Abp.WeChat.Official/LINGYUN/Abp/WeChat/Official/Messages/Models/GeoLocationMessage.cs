using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Official.Messages.Models;
/// <summary>
/// 地理位置消息
/// </summary>
[EventName("geo_location")]
public class GeoLocationMessage : WeChatOfficialGeneralMessage
{
    /// <summary>
    /// 地理位置纬度
    /// </summary>
    [XmlElement("Location_X")]
    public double Latitude { get; set; }
    /// <summary>
    /// 地理位置经度
    /// </summary>
    [XmlElement("Location_Y")]
    public double Longitude { get; set; }
    /// <summary>
    /// 地图缩放大小
    /// </summary>
    [XmlElement("Scale")]
    public double Scale { get; set; }
    /// <summary>
    /// 地理位置信息
    /// </summary>
    [XmlElement("Label")]
    public string Label { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatOfficialGeneralMessageEto<GeoLocationMessage>(this);
    }
}
