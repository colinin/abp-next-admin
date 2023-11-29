using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 弹出地理位置选择器的事件推送
/// </summary>
[EventName("location_select")]
public class GeoLocationSelectPushEevent : WeChatWorkEventMessage
{
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
    /// <summary>
    /// 发送的位置信息
    /// </summary>
    [XmlElement("SendLocationInfo")]
    public LocationInfo SendLocationInfo { get; set; }
    /// <summary>
    /// app类型，在企业微信固定返回wxwork，在微信不返回该字段
    /// </summary>
    [XmlElement("AppType")]
    public string AppType { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<GeoLocationSelectPushEevent>(this);
    }
}

public class LocationInfo
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
    /// POI的名字，可能为空
    /// </summary>
    [XmlElement("Poiname")]
    public string PoiName { get; set; }
}
