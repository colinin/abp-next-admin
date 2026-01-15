using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 申请人信息
/// </summary>
public class ApprovalApplyer : ApprovalUser
{
    /// <summary>
    /// 申请人所在部门id
    /// </summary>
    [NotNull]
    [JsonProperty("partyid")]
    [JsonPropertyName("partyid")]
    public int PartyId { get; set; }
}
