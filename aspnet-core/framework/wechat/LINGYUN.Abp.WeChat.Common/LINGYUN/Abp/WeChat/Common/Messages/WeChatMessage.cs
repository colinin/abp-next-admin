using System;
using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Common.Messages;
/// <summary>
/// 微信消息
/// </summary>
[Serializable]
[XmlRoot("xml")]
public abstract class WeChatMessage
{
    /// <summary>
    /// 开发者微信号
    /// </summary>
    [XmlElement("ToUserName")]
    public string ToUserName { get; set; }
    /// <summary>
    /// 发送方账号（一个OpenID）
    /// </summary>
    [XmlElement("FromUserName")]
    public string FromUserName { get; set; }
    /// <summary>
    /// 消息创建时间 （整型）
    /// </summary>
    [XmlElement("CreateTime")]
    public int CreateTime { get; set; }
    /// <summary>
    /// 消息类型，event
    /// </summary>
    [XmlElement("MsgType")]
    public string MsgType { get; set; }

    public abstract WeChatMessageEto ToEto();

    public virtual string SerializeToJson()
    {
        return WeChatObjectSerializeExtensions.SerializeToJson(this);
    }

    public virtual string SerializeToXml()
    {
        return this.SerializeWeChatMessage();
    }
}
