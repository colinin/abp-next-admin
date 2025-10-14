using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 流程
/// </summary>
public class ApprovalProcess
{
    /// <summary>
    /// 流程节点
    /// </summary>
    [NotNull]
    [JsonProperty("node_list")]
    [JsonPropertyName("node_list")]
    public List<ApprovalProcessNode> Nodes { get; set; }
}
