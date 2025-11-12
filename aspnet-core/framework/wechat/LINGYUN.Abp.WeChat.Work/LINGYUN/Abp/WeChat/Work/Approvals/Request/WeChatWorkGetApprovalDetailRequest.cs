using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Request;
/// <summary>
/// 获取审批申请详情请求参数
/// </summary>
/// <remarks>
/// 详情见: https://developer.work.weixin.qq.com/document/path/91983
/// </remarks>
public class WeChatWorkGetApprovalDetailRequest
{
    /// <summary>
    /// 审批单编号
    /// </summary>
    [NotNull]
    [JsonProperty("sp_no")]
    [JsonPropertyName("sp_no")]
    public string SpNo { get; set; }
    public WeChatWorkGetApprovalDetailRequest(string spNo)
    {
        SpNo = spNo;
    }
}
