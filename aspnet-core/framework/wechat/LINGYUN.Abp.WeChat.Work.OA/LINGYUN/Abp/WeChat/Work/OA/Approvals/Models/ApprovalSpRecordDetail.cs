using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 审批节点详情
/// </summary>
public class ApprovalSpRecordDetail
{
    /// <summary>
    /// 分支审批人
    /// </summary>
    [NotNull]
    [JsonProperty("approver")]
    [JsonPropertyName("approver")]
    public ApprovalUser Approver { get; set; }
    /// <summary>
    /// 审批意见
    /// </summary>
    [NotNull]
    [JsonProperty("speech")]
    [JsonPropertyName("speech")]
    public string Speech { get; set; }
    /// <summary>
    /// 分支审批人审批状态
    /// </summary>
    [NotNull]
    [JsonProperty("sp_status")]
    [JsonPropertyName("sp_status")]
    public ApprovalSpRecordStatus SpStatus { get; set; }
    /// <summary>
    /// 节点分支审批人审批操作时间戳，0表示未操作
    /// </summary>
    [NotNull]
    [JsonProperty("sptime")]
    [JsonPropertyName("sptime")]
    public long SpTime { get; set; }
    /// <summary>
    /// 节点分支审批人审批意见附件，微盘文件无法获取
    /// </summary>
    [CanBeNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public List<string> MediaId { get; set; }
}
