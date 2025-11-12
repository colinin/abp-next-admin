using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Response;
/// <summary>
/// 获取规则组详情响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94883" />
/// </remarks>
public class WeChatWorkGetCustomerStrategyResponse : WeChatWorkResponse
{
    /// <summary>
    /// 规则组详情
    /// </summary>
    [NotNull]
    [JsonProperty("strategy")]
    [JsonPropertyName("strategy")]
    public CustomerStrategyInfo Strategy { get; set; }
}
