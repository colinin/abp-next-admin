using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Request;
/// <summary>
/// 获取已服务的外部联系人请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/99434" />
/// </remarks>
public class WeChatWorkGetExternalContactListRequest : WeChatWorkRequest
{
    /// <summary>
    /// 用于分页查询的游标，字符串类型，由上一次调用返回，首次调用可不填
    /// </summary>
    /// <remarks>
    /// cursor具有有效期，请勿缓存后使用
    /// </remarks>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public string? Cursor { get; }
    /// <summary>
    /// 返回的最大记录数，整型，默认为1000
    /// </summary>
    [CanBeNull]
    [JsonProperty("limit")]
    [JsonPropertyName("limit")]
    public int? Limit { get; }
    public WeChatWorkGetExternalContactListRequest(string? cursor = null, int? limit = 1000)
    {
        Cursor = cursor;
        Limit = limit;
    }
}
