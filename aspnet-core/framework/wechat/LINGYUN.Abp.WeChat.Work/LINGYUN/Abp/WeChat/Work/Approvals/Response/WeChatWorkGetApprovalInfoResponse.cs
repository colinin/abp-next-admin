using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Response;
/// <summary>
/// 批量获取审批单号响应参数
/// </summary>
/// <remarks>
/// 详情见: https://developer.work.weixin.qq.com/document/path/91816
/// </remarks>
public class WeChatWorkGetApprovalInfoResponse : WeChatWorkResponse
{
    /// <summary>
    /// 审批单号列表，包含满足条件的审批申请
    /// </summary>
    [NotNull]
    [JsonProperty("sp_no_list")]
    [JsonPropertyName("sp_no_list")]
    public List<string> SpNos { get; set; }
    /// <summary>
    /// 后续请求查询的游标，当返回结果没有该字段时表示审批单已经拉取完
    /// </summary>
    [CanBeNull]
    [JsonProperty("new_next_cursor")]
    [JsonPropertyName("new_next_cursor")]
    public string NewNextCursor { get; set; }
}
