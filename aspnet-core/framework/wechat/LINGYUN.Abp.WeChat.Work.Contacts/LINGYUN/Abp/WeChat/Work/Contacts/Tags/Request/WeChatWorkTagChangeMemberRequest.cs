using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Tags.Request;
/// <summary>
/// 标签成员变更请求参数
/// </summary>
public class WeChatWorkTagChangeMemberRequest : WeChatWorkRequest
{
    /// <summary>
    /// 标签id，非负整型，指定此参数时新增的标签会生成对应的标签id，不指定时则以目前最大的id自增。
    /// </summary>
    [NotNull]
    [JsonProperty("tagid")]
    [JsonPropertyName("tagid")]
    public int TagId { get; set; }
    /// <summary>
    /// 企业成员ID列表，注意：userlist、partylist不能同时为空，单次请求个数不超过1000
    /// </summary>
    [CanBeNull]
    [JsonProperty("userlist")]
    [JsonPropertyName("userlist")]
    public List<string>? Users { get; set; }
    /// <summary>
    /// 	企业部门ID列表，注意：userlist、partylist不能同时为空，单次请求个数不超过100
    /// </summary>
    [CanBeNull]
    [JsonProperty("partylist")]
    [JsonPropertyName("partylist")]
    public List<int>? Parts { get; set; }
    public WeChatWorkTagChangeMemberRequest(
        int tagId,
        List<string>? users = null,
        List<int>? parts = null)
    {
        TagId = Check.Positive(tagId, nameof(tagId));
        Users = users;
        Parts = parts;

        if (users == null && parts == null)
        {
            throw new ArgumentNullException("users/parts", "userlist、partylist不能同时为空!");
        }
        if (users?.Count > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(users), "企业成员ID列表单次请求个数不超过1000!");
        }
        if (parts?.Count > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(users), "企业部门ID列表单次请求个数不超过100!");
        }
    }
}
