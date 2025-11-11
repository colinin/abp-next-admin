using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Response;
/// <summary>
/// 获取待分配的离职成员列表响应结果
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92124" />
/// </remarks>
public class WeChatWorkGetUnassignedListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 待分配离职成员的客户
    /// </summary>
    [NotNull]
    [JsonProperty("info")]
    [JsonPropertyName("info")]
    public UnassignedCustomerInfo[] Info { get; set; }
    /// <summary>
    /// 是否是最后一条记录
    /// </summary>
    [NotNull]
    [JsonProperty("is_last")]
    [JsonPropertyName("is_last")]
    public bool IsLast { get; set; }
    /// <summary>
    /// 分页查询游标,已经查完则返回空("")，使用page_id作为查询参数时不返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("next_cursor")]
    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }
}
