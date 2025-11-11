using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Response;
/// <summary>
/// 获取规则组管理范围响应参数
/// </summary>
/// <remarks>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94883" />
/// </remarks>
public class WeChatWorkGetCustomerStrategyRangeResponse : WeChatWorkResponse
{
    /// <summary>
    /// 规则组管理范围
    /// </summary>
    [NotNull]
    [JsonProperty("range")]
    [JsonPropertyName("range")]
    public CustomerStrategyRange[] Range { get; set; }
    /// <summary>
    /// 分页游标，用于查询下一个分页的数据，无更多数据时不返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("next_cursor")]
    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }
}
