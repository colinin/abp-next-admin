using LINGYUN.Abp.WeChat.Common.Messages;
using System.Collections.Generic;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 审批状态通知事件
/// </summary>
[EventName("open_approval_change")]
public class ApprovalStatusChangeEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 事件KEY值
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
    /// <summary>
    /// 审批信息
    /// </summary>
    [XmlElement("ApprovalInfo")]
    public ApprovalInfo ApprovalInfo { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<ApprovalStatusChangeEvent>(this);
    }
}

public class ApprovalInfo
{
    /// <summary>
    /// 审批单编号，由开发者在发起申请时自定义
    /// </summary>
    [XmlElement("ThirdNo")]
    public string ThirdNo { get; set; }
    /// <summary>
    /// 审批模板名称
    /// </summary>
    [XmlElement("OpenSpName")]
    public string OpenSpName { get; set; }
    /// <summary>
    /// 审批模板id
    /// </summary>
    [XmlElement("OpenTemplateId")]
    public string OpenTemplateId { get; set; }
    /// <summary>
    /// 申请单当前审批状态：
    /// 1-审批中；
    /// 2-已通过；
    /// 3-已驳回；
    /// 4-已取消
    /// </summary>
    [XmlElement("OpenSpStatus")]
    public byte OpenSpStatus { get; set; }
    /// <summary>
    /// 提交申请时间
    /// </summary>
    [XmlElement("ApplyTime")]
    public int ApplyTime { get; set; }
    /// <summary>
    /// 提交者姓名
    /// </summary>
    [XmlElement("ApplyUserName")]
    public string ApplyUserName { get; set; }
    /// <summary>
    /// 提交者userid
    /// </summary>
    [XmlElement("ApplyUserId")]
    public string ApplyUserId { get; set; }
    /// <summary>
    /// 提交者所在部门
    /// </summary>
    [XmlElement("ApplyUserParty")]
    public string ApplyUserParty { get; set; }
    /// <summary>
    /// 提交者头像
    /// </summary>
    [XmlElement("ApplyUserImage")]
    public string ApplyUserImage { get; set; }
    /// <summary>
    /// 审批流程信息
    /// </summary>
    [XmlElement("ApprovalNodes")]
    public List<ApprovalNode> ApprovalNodes { get; set; }
    /// <summary>
    /// 抄送信息，可能有多个抄送人
    /// </summary>
    [XmlElement("NotifyNodes")]
    public List<NotifyNode> NotifyNodes { get; set; }
    /// <summary>
    /// 当前审批节点：0-第一个审批节点；1-第二个审批节点…以此类推
    /// </summary>
    [XmlElement("approverstep")]
    public int ApproverStep { get; set; }
}

/// <summary>
/// 审批流程信息，可以有多个审批节点
/// </summary>
[XmlRoot("ApprovalNode")]
public class ApprovalNode
{
    /// <summary>
    /// 节点审批操作状态：
    /// 1-审批中；
    /// 2-已同意；
    /// 3-已驳回；
    /// 4-已转审
    /// </summary>
    [XmlElement("NodeStatus")]
    public byte NodeStatus { get; set; }
    /// <summary>
    /// 审批节点属性：
    /// 1-或签；
    /// 2-会签
    /// </summary>
    [XmlElement("NodeAttr")]
    public byte NodeAttr { get; set; }
    /// <summary>
    /// 审批节点类型：
    /// 1-固定成员；
    /// 2-标签；
    /// 3-上级
    /// </summary>
    [XmlElement("NodeType")]
    public byte NodeType { get; set; }
    /// <summary>
    /// 审批节点信息，当节点为标签或上级时，一个节点可能有多个分支
    /// </summary>
    [XmlElement("Items")]
    public List<ApprovalNodeItem> Items { get; set; }
}
/// <summary>
/// 审批节点分支，当节点为标签或上级时，一个节点可能有多个分支
/// </summary>
[XmlRoot("Item")]
public class ApprovalNodeItem
{
    /// <summary>
    /// 分支审批人姓名
    /// </summary>
    [XmlElement("ItemName")]
    public string ItemName { get; set; }
    /// <summary>
    /// 分支审批人userid
    /// </summary>
    [XmlElement("ItemUserId")]
    public string ItemUserId { get; set; }
    /// <summary>
    /// 分支审批人头像
    /// </summary>
    [XmlElement("ItemImage")]
    public string ItemImage { get; set; }
    /// <summary>
    /// 分支审批审批操作状态：
    /// 1-审批中；
    /// 2-已同意；
    /// 3-已驳回；
    /// 4-已转审
    /// </summary>
    [XmlElement("ItemStatus")]
    public byte ItemStatus { get; set; }
    /// <summary>
    /// 分支审批人审批意见
    /// </summary>
    [XmlElement("ItemSpeech")]
    public string ItemSpeech { get; set; }
    /// <summary>
    /// 分支审批人审批意见
    /// </summary>
    [XmlElement("ItemOpTime")]
    public int ItemOpTime { get; set; }
}
/// <summary>
/// 抄送人信息
/// </summary>
[XmlRoot("NotifyNode")]
public class NotifyNode
{
    /// <summary>
    /// 抄送人姓名
    /// </summary>
    [XmlElement("ItemName")]
    public string ItemName { get; set; }
    /// <summary>
    /// 抄送人userid
    /// </summary>
    [XmlElement("ItemUserId")]
    public string ItemUserId { get; set; }
    /// <summary>
    /// 抄送人头像
    /// </summary>
    [XmlElement("ItemImage")]
    public string ItemImage { get; set; }
}