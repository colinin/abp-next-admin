using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
/// <summary>
/// 创建新的规则组请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94883#%E5%88%9B%E5%BB%BA%E6%96%B0%E7%9A%84%E8%A7%84%E5%88%99%E7%BB%84" />
/// </remarks>
public class WeChatWorkBulkGetCustomerRequest : WeChatWorkRequest
{
    /// <summary>
    /// 企业成员的userid列表，字符串类型，最多支持100个
    /// </summary>
    [NotNull]
    [JsonProperty("userid_list")]
    [JsonPropertyName("userid_list")]
    public List<string> UserIds { get; }
    /// <summary>
    /// 用于分页查询的游标，字符串类型，由上一次调用返回，首次调用可不填
    /// </summary>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public string? Cursor { get; }
    /// <summary>
    /// 返回的最大记录数，整型，最大值100，默认值50，超过最大值时取最大值
    /// </summary>
    [CanBeNull]
    [JsonProperty("limit")]
    [JsonPropertyName("limit")]
    public int Limit { get; }
    public WeChatWorkBulkGetCustomerRequest(
        List<string> userIds, 
        string? cursor = null, 
        int limit = 50)
    {
        Check.NotNullOrEmpty(userIds, nameof(userIds));
        Check.Range(limit, nameof(limit), 1, 100);

        if (userIds.Count > 100)
        {
            throw new ArgumentException("The maximum number of userIds allowed in the list is only 100!");
        }

        UserIds = userIds;
        Cursor = cursor;
        Limit = limit;
    }
}
