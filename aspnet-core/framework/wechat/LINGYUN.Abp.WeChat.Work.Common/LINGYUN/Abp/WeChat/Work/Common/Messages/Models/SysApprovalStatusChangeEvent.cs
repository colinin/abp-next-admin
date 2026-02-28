using LINGYUN.Abp.WeChat.Common.Messages;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 企业微信“审批应用”审批状态通知事件
/// </summary>
[EventName("sys_approval_change")]
public class SysApprovalStatusChangeEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 审批信息
    /// </summary>
    [XmlElement("ApprovalInfo")]
    public SysApprovalInfo ApprovalInfo { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<SysApprovalStatusChangeEvent>(this);
    }
}

public class SysApprovalInfo
{
    /// <summary>
    /// 审批编号（字符串类型）
    /// </summary>
    [XmlElement("SpNoStr")]
    public string SpNoStr { get; set; }
    /// <summary>
    /// 审批申请类型名称（审批模板名称）
    /// </summary>
    [XmlElement("SpName")]
    public string SpName { get; set; }
    /// <summary>
    /// 申请单状态：1-审批中；2-已通过；3-已驳回；4-已撤销；6-通过后撤销；7-已删除；10-已支付
    /// </summary>
    [XmlElement("SpStatus")]
    public byte SpStatus { get; set; }
    /// <summary>
    /// 审批模板id。可在“获取审批申请详情”、“审批状态变化回调通知”中获得，也可在审批模板的模板编辑页面链接中获得。
    /// </summary>
    [XmlElement("TemplateId")]
    public string TemplateId { get; set; }
    /// <summary>
    /// 审批申请提交时间,Unix时间戳
    /// </summary>
    [XmlElement("ApplyTime")]
    public int ApplyTime { get; set; }
    /// <summary>
    /// 申请人信息
    /// </summary>
    [XmlElement("Applyer")]
    public SysApprovalApplyer Applyer { get; set; }
    /// <summary>
    /// 审批流程信息，可能有多个审批节点。
    /// </summary>
    [XmlElement("SpRecord")]
    public List<SysApprovalRecord> SpRecord { get; set; }
    /// <summary>
    /// 抄送信息，可能有多个抄送节点
    /// </summary>
    [XmlElement("Notifyer")]
    public List<SysApprovalNotifyer> Notifyer { get; set; }
    /// <summary>
    /// 审批申请备注信息，可能有多个备注节点
    /// </summary>
    [XmlElement("Comments")]
    public List<SysApprovalComment> Comments { get; set; }
    /// <summary>
    /// 审批流程列表
    /// </summary>
    [XmlElement("ProcessList")]
    public List<SysApprovalProcess> ProcessList { get; set; }
    /// <summary>
    /// 审批申请状态变化类型：1-提单；2-同意；3-驳回；4-转审；5-催办；6-撤销；8-通过后撤销；10-添加备注；11-回退给指定审批人；12-添加审批人；13-加签并同意； 14-已办理； 15-已转交
    /// </summary>
    [XmlElement("StatuChangeEvent")]
    public byte StatuChangeEvent { get; set; }
    /// <summary>
    /// 审批编号
    /// </summary>
    /// <remarks>
    /// 局校审批单不返回此字段，其他类型审批单会返回此字段，不推荐使用此字段
    /// </remarks>
    [XmlElement("SpNo")]
    [Obsolete("局校审批单不返回此字段，其他类型审批单会返回此字段，不推荐使用此字段")]
    public string SpNo { get; set; }
}

public class SysApprovalApplyer
{
    /// <summary>
    /// 申请人userid
    /// </summary>
    [XmlElement("UserId")]
    public string UserId { get; set; }
    /// <summary>
    /// 申请人所在部门pid
    /// </summary>
    [XmlElement("Party")]
    public string Party { get; set; }
}

public class SysApprovalRecord
{
    /// <summary>
    /// 审批节点状态：1-审批中；2-已同意；3-已驳回；4-已转审
    /// </summary>
    [XmlElement("SpStatus")]
    public byte SpStatus { get; set; }
    /// <summary>
    /// 节点审批方式：1-或签；2-会签
    /// </summary>
    [XmlElement("ApproverAttr")]
    public byte ApproverAttr { get; set; }
    /// <summary>
    /// 节点审批方式：1-或签；2-会签
    /// </summary>
    [XmlElement("Details")]
    public List<SysApprovalRecordDetail> Details { get; set; }
}

public class SysApprovalRecordDetail
{
    /// <summary>
    /// 分支审批人
    /// </summary>
    [XmlElement("Approver")]
    public SysApprovalApplyer Approver { get; set; }
    /// <summary>
    /// 审批意见字段
    /// </summary>
    [XmlElement("Speech")]
    public string Speech { get; set; }
    /// <summary>
    /// 分支审批人审批状态：1-审批中；2-已同意；3-已驳回；4-已转审
    /// </summary>
    [XmlElement("SpStatus")]
    public byte SpStatus { get; set; }
    /// <summary>
    /// 节点分支审批人审批操作时间，0为尚未操作
    /// </summary>
    [XmlElement("SpTime")]
    public int SpTime { get; set; }
    /// <summary>
    /// 节点分支审批人审批意见附件，赋值为media_id具体使用请参考：文档-获取临时素材
    /// </summary>
    [XmlElement("Attach")]
    public string Attach { get; set; }
}

public class SysApprovalNotifyer
{
    /// <summary>
    /// 节点抄送人userid
    /// </summary>
    [XmlElement("UserId")]
    public string UserId { get; set; }
}

public class SysApprovalComment
{
    /// <summary>
    /// 备注人信息
    /// </summary>
    [XmlElement("CommentUserInfo")]
    public SysApprovalCommenter CommentUserInfo { get; set; }
    /// <summary>
    /// 备注提交时间
    /// </summary>
    [XmlElement("CommentTime")]
    public int CommentTime { get; set; }
    /// <summary>
    /// 备注文本内容
    /// </summary>
    [XmlElement("CommentContent")]
    public string CommentContent { get; set; }
    /// <summary>
    /// 备注id
    /// </summary>
    [XmlElement("CommentId")]
    public string CommentId { get; set; }
    /// <summary>
    /// 备注意见附件，值是附件media_id
    /// </summary>
    [XmlElement("Attach")]
    public string Attach { get; set; }
}

public class SysApprovalCommenter
{
    /// <summary>
    /// 节点抄送人userid
    /// </summary>
    [XmlElement("UserId")]
    public string UserId { get; set; }
}

public class SysApprovalProcess
{
    /// <summary>
    /// 流程节点
    /// </summary>
    [XmlElement("NodeList")]
    public List<SysApprovalProcessNode> NodeList { get; set; }
}

public class SysApprovalProcessNode
{
    /// <summary>
    /// 节点类型 1 审批人 2 抄送人 3办理人
    /// </summary>
    [XmlElement("NodeType")]
    public byte NodeType { get; set; }
    /// <summary>
    /// 节点状态 1-审批中；2-同意；3-驳回；4-转审；11-退回给指定审批人；12-加签；13-同意并加签；14-办理；15-转交
    /// </summary>
    [XmlElement("SpStatus")]
    public byte SpStatus { get; set; }
    /// <summary>
    /// 多人办理方式 1-会签；2-或签 3-依次审批
    /// </summary>
    [XmlElement("ApvRel")]
    public byte ApvRel { get; set; }
    /// <summary>
    /// 子节点列表
    /// </summary>
    [XmlElement("SubNodeList")]
    public List<SysApprovalProcessSubNode> SubNodeList { get; set; }
}

public class SysApprovalProcessSubNode
{
    /// <summary>
    /// 处理人信息
    /// </summary>
    [XmlElement("UserInfo")]
    public SysApprovalProcesser UserInfo { get; set; }
    /// <summary>
    /// 审批/办理意见
    /// </summary>
    [XmlElement("Speech")]
    public string Speech { get; set; }
    /// <summary>
    /// 子节点状态 1-审批中；2-同意；3-驳回；4-转审；11-退回给指定审批人；12-加签；13-同意并加签；14-办理；15-转交
    /// </summary>
    [XmlElement("SpYj")]
    public byte SpYj { get; set; }
    /// <summary>
    /// 操作时间
    /// </summary>
    [XmlElement("Sptime")]
    public int Sptime { get; set; }
    /// <summary>
    /// 备注意见附件，值是附件media_id
    /// </summary>
    [XmlElement("MediaIds")]
    public string MediaIds { get; set; }
}

public class SysApprovalProcesser
{
    /// <summary>
    /// 处理人userid	
    /// </summary>
    [XmlElement("UserId")]
    public string UserId { get; set; }
}