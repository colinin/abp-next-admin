using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 属性类型
/// </summary>
[Description("属性类型")]
public enum AttributeType
{
    /// <summary>
    /// 文本
    /// </summary>
    [Description("文本")]
    Text = 0,
    /// <summary>
    /// 网页
    /// </summary>
    [Description("网页")]
    Web = 1
}
