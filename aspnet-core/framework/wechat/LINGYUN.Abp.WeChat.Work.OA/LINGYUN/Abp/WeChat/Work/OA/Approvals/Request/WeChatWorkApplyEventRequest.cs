using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Request;
/// <summary>
/// 提交审批申请请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91853"/>
/// </remarks>
public class WeChatWorkApplyEventRequest : WeChatWorkRequest
{
    /// <summary>
    /// 流程
    /// </summary>
    /// <remarks>
    /// use_template_approver = 0 时必填
    /// </remarks>
    [CanBeNull]
    [JsonProperty("process")]
    [JsonPropertyName("process")]
    public ApprovalApplyProcess? Process { get; set; }
    /// <summary>
    /// 摘要信息，用于显示在审批通知卡片、审批列表的摘要信息，最多3行
    /// </summary>
    [NotNull]
    [JsonProperty("summary_list")]
    [JsonPropertyName("summary_list")]
    public List<ApprovalSummary> Summaries { get; set; }
    /// <summary>
    /// 审批申请数据，可定义审批申请中各个控件的值，其中必填项必须有值，选填项可为空，数据结构同“获取审批申请详情”接口返回值中同名参数“apply_data”
    /// </summary>
    [NotNull]
    [JsonProperty("apply_data")]
    [JsonPropertyName("apply_data")]
    public ApprovalApplyData ApplyData { get; set; }
    /// <summary>
    /// 模板id。可在“获取审批申请详情”、“审批状态变化回调通知”中获得，也可在审批模板的模板编辑页面链接中获得。暂不支持通过接口提交[打卡补卡][调班]模板审批单。
    /// </summary>
    [NotNull]
    [JsonProperty("template_id")]
    [JsonPropertyName("template_id")]
    public string TemplateId { get; set; }
    /// <summary>
    /// 申请人userid，此审批申请将以此员工身份提交，申请人需在应用可见范围内
    /// </summary>
    [NotNull]
    [JsonProperty("creator_userid")]
    [JsonPropertyName("creator_userid")]
    public string CreatorUserId { get; set; }
    /// <summary>
    /// 审批人模式
    /// </summary>
    /// <remarks>
    /// 0-通过接口指定审批人、抄送人（此时process参数必填）; <br />
    /// 1-使用此模板在管理后台设置的审批流程(需要保证审批流程中没有“申请人自选”节点)，支持条件审批。默认为0
    /// </remarks>
    [NotNull]
    [JsonProperty("use_template_approver")]
    [JsonPropertyName("use_template_approver")]
    public byte UseTemplateApprover 
    {
        get {
            if (Process == null)
            {
                return 1;
            }
            return 0;
        }
    }
    /// <summary>
    /// 提单者提单部门id，不填默认为主部门
    /// </summary>
    [CanBeNull]
    [JsonProperty("choose_department")]
    [JsonPropertyName("choose_department")]
    public byte? ChooseDepartment { get; set; }
    public WeChatWorkApplyEventRequest(
        string templateId,
        string creatorUserid,
        ApprovalApplyData applyData,
        ApprovalApplyProcess? process = null,
        byte? chooseDepartment = null)
    {
        TemplateId = templateId;
        ApplyData = applyData;
        CreatorUserId = creatorUserid;
        ChooseDepartment = chooseDepartment;
        Process = process;
    }
}
