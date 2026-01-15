using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
/// <summary>
/// 邀请成员请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90975" />
/// </remarks>
public class WeChatWorkBulkInviteMemberRequest : WeChatWorkRequest
{
    /// <summary>
    /// 成员ID列表, 最多支持1000个。
    /// </summary>
    [NotNull]
    [JsonProperty("user")]
    [JsonPropertyName("user")]
    public List<string> User { get; }
    /// <summary>
    /// 部门ID列表，最多支持100个。
    /// </summary>
    [NotNull]
    [JsonProperty("party")]
    [JsonPropertyName("party")]
    public List<int> Party { get; }
    /// <summary>
    /// 标签ID列表，最多支持100个。
    /// </summary>
    [NotNull]
    [JsonProperty("tag")]
    [JsonPropertyName("tag")]
    public List<int> Tag { get; }
    public WeChatWorkBulkInviteMemberRequest()
    {
        User = new List<string>();
        Party = new List<int>();
        Tag = new List<int>();
    }

    protected override void Validate()
    {
        if (User.IsNullOrEmpty() && Party.IsNullOrEmpty() && Tag.IsNullOrEmpty())
        {
            throw new ArgumentException("User, Party, and Tag cannot all be empty at the same time!");
        }
        if (User.Count > 1000)
        {
            throw new ArgumentException("User list, up to 1000 supported!");
        }
        if (Party.Count > 100)
        {
            throw new ArgumentException("Party list, up to 100 supported!");
        }
        if (Tag.Count > 100)
        {
            throw new ArgumentException("Tag list, up to 100 supported!");
        }
    }
}
