using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Official.Messages.Models;
/// <summary>
/// 上报地理位置事件
/// </summary>
[EventName("reporting_geo_location")]
public class ReportingGeoLocationEvent : WeChatEventMessage
{
    /// <summary>
    /// 地理位置纬度
    /// </summary>
    [XmlElement("Latitude")]
    public double Latitude { get; set; }
    /// <summary>
    /// 地理位置经度
    /// </summary>
    [XmlElement("Longitude")]
    public double Longitude { get; set; }
    /// <summary>
    /// 地理位置精度
    /// </summary>
    [XmlElement("Precision")]
    public double Precision { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatOfficialEventMessageEto<ReportingGeoLocationEvent>(this);
    }
}
