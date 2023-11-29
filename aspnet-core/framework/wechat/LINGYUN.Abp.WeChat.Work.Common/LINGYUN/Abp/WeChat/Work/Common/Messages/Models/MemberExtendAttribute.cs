using System.Collections.Generic;
using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;

[XmlRoot("Item")]
public class MemberExtendAttribute
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    [XmlElement("Item")]
    public List<MemberExtend> Items { get;set; }
}

public class MemberExtend
{
    /// <summary>
    /// 扩展属性类型: 0-文本 1-网页
    /// </summary>
    [XmlElement("Type")]
    public byte Type { get; set; }
    /// <summary>
    /// 扩展属性名称
    /// </summary>
    [XmlElement("Name")]
    public string Name { get; set; }
    /// <summary>
    /// 文本属性内容
    /// </summary>
    [XmlElement("Text", IsNullable = true)]
    public MemberTextExtend Text { get; set; }
    /// <summary>
    /// Web属性内容
    /// </summary>
    [XmlElement("Web", IsNullable = true)]
    public MemberWebExtend Web { get; set; }
}

/// <summary>
/// 文本属性类型，扩展属性类型为0时填写
/// </summary>
[XmlRoot("Text")]
public class MemberTextExtend 
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
public class MemberWebExtend
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
