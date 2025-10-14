using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Approvals.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Response;
/// <summary>
/// 获取审批申请详情响应参数
/// </summary>
/// <remarks>
/// 详情见: https://developer.work.weixin.qq.com/document/path/91983
/// </remarks>
public class WeChatWorkGetApprovalDetailResponse : WeChatWorkResponse
{
    /// <summary>
    /// 审批申请详情
    /// </summary>
    [NotNull]
    [JsonProperty("info")]
    [JsonPropertyName("info")]
    public ApprovalDetailInfo Info { get; set; }
}
