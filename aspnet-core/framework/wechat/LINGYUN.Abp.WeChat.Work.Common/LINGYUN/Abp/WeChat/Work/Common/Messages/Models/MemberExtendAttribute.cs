using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
[XmlRoot("Item")]
public class MemberExtendAttribute
{
    /// <summary>
    /// 扩展属性类型: 0-本文 1-网页
    /// </summary>
    [XmlElement("Type")]
    public byte Type {  get; set; }
    /// <summary>
    /// 扩展属性类型: 0-本文 1-网页
    /// </summary>
    public MemberExtend Extend { get; set; }
}

public abstract class MemberExtend
{
}

/// <summary>
/// 文本属性类型，扩展属性类型为0时填写
/// </summary>
[XmlRoot("Text")]
public class MemberTextExtend : MemberExtend
{
    /// <summary>
    /// 文本属性内容
    /// </summary>
    [XmlElement("Value")]
    public string Value { get; set;}
}
/// <summary>
/// 网页类型属性，扩展属性类型为1时填写
/// </summary>
[XmlRoot("Web")]
public class MemberWebExtend : MemberExtend
{
    /// <summary>
    /// 网页的展示标题
    /// </summary>
    [XmlElement("Title")]
    public string Title { get; set; }
    /// <summary>
    /// 网页的url
    /// </summary>
    [XmlElement("Url")]
    public string Url { get; set; }
}
