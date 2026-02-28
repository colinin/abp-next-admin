using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Response;
/// <summary>
/// 批量获取客户详情响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92994" />
/// </remarks>
public class WeChatWorkBulkGetCustomerResponse : WeChatWorkResponse
{
    /// <summary>
    /// 外部联系人的userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("external_contact_list")]
    [JsonPropertyName("external_contact_list")]
    public List<ExternalContactList> ExternalUserId { get; set; } = new List<ExternalContactList>();
    /// <summary>
    /// 分页游标，再下次请求时填写以获取之后分页的记录，如果已经没有更多的数据则返回空
    /// </summary>
    [CanBeNull]
    [JsonProperty("next_cursor")]
    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }
    /// <summary>
    /// 若请求中所有userid都无有效互通许可，接口直接报错701008。如果部分userid无有效互通许可，接口返回成功
    /// </summary>
    [CanBeNull]
    [JsonProperty("fail_info")]
    [JsonPropertyName("fail_info")]
    public WeChatWorkBulkGetCustomerFailInfo? FailInfo { get; set; }
}

public class WeChatWorkBulkGetCustomerFailInfo
{
    /// <summary>
    /// 无许可的userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("next_cursor")]
    [JsonPropertyName("next_cursor")]
    public List<string> UnlicensedUseridList { get; set; } = new List<string>();
}