using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Response;
/// <summary>
/// 提交审批申请响应参数
/// </summary>
/// <remarks>
/// 详情见: https://developer.work.weixin.qq.com/document/path/91853
/// </remarks>
public class WeChatWorkApplyEventResponse : WeChatWorkResponse
{
    /// <summary>
    /// 表单提交成功后，返回的表单编号
    /// </summary>
    [NotNull]
    [JsonProperty("sp_no")]
    [JsonPropertyName("sp_no")]
    public string SpNo { get; set; }
}
