using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 审批申请详情
/// </summary>
public class ApprovalDetailInfo
{
    /// <summary>
    /// 审批编号
    /// </summary>
    [NotNull]
    [JsonProperty("sp_no")]
    [JsonPropertyName("sp_no")]
    public string SpNo { get; set; }
    /// <summary>
    /// 审批申请类型名称（审批模板名称）
    /// </summary>
    [NotNull]
    [JsonProperty("sp_name")]
    [JsonPropertyName("sp_name")]
    public string SpName { get; set; }
    /// <summary>
    /// 申请单状态
    /// </summary>
    [NotNull]
    [JsonProperty("sp_status")]
    [JsonPropertyName("sp_status")]
    public ApprovalSpStatus SpStatus { get; set; }
    /// <summary>
    /// 审批模板id。可在“获取审批申请详情”、“审批状态变化回调通知”中获得，也可在审批模板的模板编辑页面链接中获得。
    /// </summary>
    [NotNull]
    [JsonProperty("template_id")]
    [JsonPropertyName("template_id")]
    public string TemplateId { get; set; }
    /// <summary>
    /// 审批申请提交时间,Unix时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("apply_time")]
    [JsonPropertyName("apply_time")]
    public long ApplyTime { get; set; }
    /// <summary>
    /// 申请人信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("applyer")]
    [JsonPropertyName("applyer")]
    public ApprovalApplyer Applyer { get; set; }
    /// <summary>
    /// 批量申请人信息（和applyer字段互斥）
    /// </summary>
    [CanBeNull]
    [JsonProperty("batch_applyer")]
    [JsonPropertyName("batch_applyer")]
    public List<ApprovalUser> BatchApplyer { get; set; }
    /// <summary>
    /// 审批流程信息，可能有多个审批节点
    /// </summary>
    [NotNull]
    [JsonProperty("sp_record")]
    [JsonPropertyName("sp_record")]
    public List<ApprovalSpRecord> SpRecord { get; set; }
    /// <summary>
    /// 抄送信息，可能有多个抄送节点
    /// </summary>
    [CanBeNull]
    [JsonProperty("notifyer")]
    [JsonPropertyName("notifyer")]
    public List<ApprovalUser> Notifyer { get; set; }
    /// <summary>
    /// 审批申请数据
    /// </summary>
    [NotNull]
    [JsonProperty("apply_data")]
    [JsonPropertyName("apply_data")]
    public ApprovalData ApplyData { get; set; }
    /// <summary>
    /// 审批申请备注信息，可能有多个备注节点
    /// </summary>
    [NotNull]
    [JsonProperty("comments")]
    [JsonPropertyName("comments")]
    public List<ApprovalComment> Comments { get; set; }
    /// <summary>
    /// 审批流程列表
    /// </summary>
    [NotNull]
    [JsonProperty("process_list")]
    [JsonPropertyName("process_list")]
    public ApprovalProcess Process { get; set; }
}
