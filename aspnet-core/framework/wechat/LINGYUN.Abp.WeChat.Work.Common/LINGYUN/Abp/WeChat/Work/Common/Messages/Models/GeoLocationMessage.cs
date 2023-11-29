using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 地理位置消息
/// </summary>
[EventName("geo_location")]
public class GeoLocationMessage : WeChatWorkGeneralMessage
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
    /// <summary>
    /// app类型，在企业微信固定返回wxwork，在微信不返回该字段
    /// </summary>
    [XmlElement("AppType")]
    public string AppType { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkGeneralMessageEto<GeoLocationMessage>(this);
    }
}
