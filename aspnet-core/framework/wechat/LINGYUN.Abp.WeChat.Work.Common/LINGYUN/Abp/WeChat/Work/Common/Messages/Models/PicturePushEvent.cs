using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 弹出系统发图的事件推送
/// </summary>
public abstract class PicturePushEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("SendPicsInfo")]
    public PictureInfo SendPicsInfo { get; set; }
}

public class PictureInfo
{
    /// <summary>
    /// 发送的图片数量
    /// </summary>
    [XmlElement("Count")]
    public int Count { get; set; }
    /// <summary>
    /// 发送的图片数量
    /// </summary>
    [XmlArrayItem("Item")]
    public Picture Picture { get; set; }
}

[XmlRoot("PicList")]
public class Picture
{
    /// <summary>
    /// 图片的MD5值，开发者若需要，可用于验证接收到图片
    /// </summary>
    [XmlElement("PicMd5Sum")]
    public string PicMd5Sum { get; set; }
}
