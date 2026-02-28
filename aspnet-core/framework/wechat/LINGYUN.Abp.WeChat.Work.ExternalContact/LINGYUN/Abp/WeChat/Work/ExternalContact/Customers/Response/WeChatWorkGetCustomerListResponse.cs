using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Response;
/// <summary>
/// 获取客户列表响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92113" />
/// </remarks>
public class WeChatWorkGetCustomerListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 外部联系人的userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public List<string> ExternalUserId { get; set; } = new List<string>();
}
