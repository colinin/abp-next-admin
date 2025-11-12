using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 摘要信息
/// </summary>
public class ApprovalSummary
{
    /// <summary>
    /// 摘要行信息，用于定义某一行摘要显示的内容
    /// </summary>
    [NotNull]
    [JsonProperty("summary_info")]
    [JsonPropertyName("summary_info")]
    public List<ApprovalSummaryInfo> SummaryInfo { get; set; }
    public ApprovalSummary()
    {

    }

    public ApprovalSummary(List<ApprovalSummaryInfo> summaryInfo)
    {
        SummaryInfo = summaryInfo;
    }
}
