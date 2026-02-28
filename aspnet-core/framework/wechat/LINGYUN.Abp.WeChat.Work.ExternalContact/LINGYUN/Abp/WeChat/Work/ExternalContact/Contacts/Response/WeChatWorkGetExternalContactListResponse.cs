using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Response;
/// <summary>
/// 获取已服务的外部联系人响应结果
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92994" />
/// </remarks>
public class WeChatWorkGetExternalContactListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 外部联系人列表
    /// </summary>
    [NotNull]
    [JsonProperty("info_list")]
    [JsonPropertyName("info_list")]
    public ExternalContactInfo[] InfoList { get; set; }
    /// <summary>
    /// 分页游标，再下次请求时填写以获取之后分页的记录，如果已经没有更多的数据则返回空,有效期为4小时
    /// </summary>
    [CanBeNull]
    [JsonProperty("next_cursor")]
    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }
}
