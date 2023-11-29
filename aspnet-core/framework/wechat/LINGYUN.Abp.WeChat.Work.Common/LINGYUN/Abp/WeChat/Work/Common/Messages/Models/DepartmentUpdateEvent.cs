using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 部门变更事件
/// </summary>
public abstract class DepartmentUpdateEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 部门Id
    /// </summary>
    [XmlElement("Id")]
    public int Id { get; set; }
    /// <summary>
    /// 部门名称
    /// </summary>
    [XmlElement("Name")]
    public int Name { get; set; }
    /// <summary>
    /// 父部门id
    /// </summary>
    [XmlElement("ParentId", IsNullable = true)]
    public int? ParentId { get; set; }
}
