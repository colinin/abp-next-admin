using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 企业客户标签变更事件推送
/// </summary>
public abstract class ExternalTagChangeEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 变更类型
    /// </summary>
    [XmlElement("ChangeType")]
    public string ChangeType { get; set; }
    /// <summary>
    /// 标签或标签组所属的规则组id，只回调给“客户联系”应用
    /// </summary>
    [XmlElement("StrategyId")]
    public string StrategyId { get; set; }
}
