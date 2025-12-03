using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
/// <summary>
/// 获取成员ID列表请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/96067" />
/// </remarks>
public class WeChatWorkGetUserIdListRequest : WeChatWorkRequest
{
    /// <summary>
    /// 用于分页查询的游标，字符串类型，由上一次调用返回，首次调用不填
    /// </summary>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }
    /// <summary>
    /// 分页，预期请求的数据量，取值范围 1 ~ 10000
    /// </summary>
    [CanBeNull]
    [JsonProperty("limit")]
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }
    public WeChatWorkGetUserIdListRequest(string? cursor = null, int? limit = 100)
    {
        Cursor = cursor;
        Limit = limit;
    }

    protected override void Validate()
    {
        if (Limit.HasValue)
        {
            Check.Range(Limit.Value, nameof(Limit), 1, 10000);
        }
    }
}
