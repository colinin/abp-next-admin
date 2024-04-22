using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 上报地理位置事件
/// </summary>
[EventName("reporting_geo_location")]
public class ReportingGeoLocationEvent : WeChatWorkEventMessage
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
    /// <summary>
    /// app类型，在企业微信固定返回wxwork，在微信不返回该字段
    /// </summary>
    [XmlElement("AppType")]
    public string AppType { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ReportingGeoLocationEvent>(this);
    }
}
