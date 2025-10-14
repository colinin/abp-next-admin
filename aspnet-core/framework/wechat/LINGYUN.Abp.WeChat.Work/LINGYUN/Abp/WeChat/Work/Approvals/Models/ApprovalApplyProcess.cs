using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 申请流程
/// </summary>
public class ApprovalApplyProcess
{
    /// <summary>
    /// 流程节点列表
    /// </summary>
    [NotNull]
    [JsonProperty("node_list")]
    [JsonPropertyName("node_list")]
    public List<ApprovalApplyProcessNode> Nodes { get; set; }
    public ApprovalApplyProcess()
    {

    }

    public ApprovalApplyProcess(List<ApprovalApplyProcessNode> nodes)
    {
        Nodes = nodes;
    }
}
