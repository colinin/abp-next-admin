using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 审批申请单数据
/// </summary>
public class ApprovalControlValue
{
    /// <summary>
    /// 文本内容，即申请人在此控件填写的文本内容
    /// </summary>
    [CanBeNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public string Text { get; set; }
    /// <summary>
    /// 数字内容，即申请人在此控件填写的数字内容
    /// </summary>
    [CanBeNull]
    [JsonProperty("new_number")]
    [JsonPropertyName("new_number")]
    public decimal? NewNumber { get; set; }
    /// <summary>
    /// 金额内容，即申请人在此控件填写的金额内容
    /// </summary>
    [CanBeNull]
    [JsonProperty("new_money")]
    [JsonPropertyName("new_money")]
    public decimal? NewMoney { get; set; }
    /// <summary>
    /// 所选成员内容，即申请人在此控件选择的成员，多选模式下可以有多个
    /// </summary>
    [NotNull]
    [JsonProperty("members")]
    [JsonPropertyName("members")]
    public List<MemberValue> Members { get; set; }
    /// <summary>
    /// 所选部门内容，即申请人在此控件选择的部门，多选模式下可能有多个
    /// </summary>
    [NotNull]
    [JsonProperty("departments")]
    [JsonPropertyName("departments")]
    public List<DepartmentValue> Departments { get; set; }
    /// <summary>
    /// 附件列表
    /// </summary>
    [NotNull]
    [JsonProperty("files")]
    [JsonPropertyName("files")]
    public List<FileValue> Files { get; set; }
    /// <summary>
    /// 明细内容，一个明细控件可能包含多个子明细
    /// </summary>
    [NotNull]
    [JsonProperty("children")]
    [JsonPropertyName("children")]
    public List<ApprovalDataChildrenValue> Children { get; set; }
    /// <summary>
    /// 关联审批单
    /// </summary>
    [NotNull]
    [JsonProperty("related_approval")]
    [JsonPropertyName("related_approval")]
    public List<RelatedApprovalValue> RelatedApproval { get; set; }
}

public class ApprovalDataChildrenValue
{
    /// <summary>
    /// 明细内容，一个明细控件可能包含多个子明细
    /// </summary>
    [NotNull]
    [JsonProperty("list")]
    [JsonPropertyName("list")]
    public List<ApprovalControlData> List { get; set; }
}