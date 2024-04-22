using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 用户扫码事件
/// </summary>
public abstract class ScanCodeEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
    /// <summary>
    /// 扫描信息
    /// </summary>
    [XmlElement("ScanCodeInfo")]
    public ScanCodeInfo ScanCodeInfo { get; set; }
}

public class ScanCodeInfo
{
    /// <summary>
    /// 扫描类型，一般是qrcode
    /// </summary>
    [XmlElement("ScanType")]
    public string ScanType { get; set; }
    /// <summary>
    /// 扫描结果，即二维码对应的字符串信息
    /// </summary>
    [XmlElement("ScanResult")]
    public string ScanResult { get; set; }
}
