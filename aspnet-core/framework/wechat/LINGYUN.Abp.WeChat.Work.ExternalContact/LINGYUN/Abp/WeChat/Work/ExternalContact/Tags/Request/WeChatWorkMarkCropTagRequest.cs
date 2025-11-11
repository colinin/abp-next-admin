using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Request;
/// <summary>
/// 编辑客户企业标签请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92118" />
/// </remarks>
public class WeChatWorkMarkCropTagRequest : WeChatWorkRequest
{
    /// <summary>
    /// 添加外部联系人的userid
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; }
    /// <summary>
    /// 外部联系人userid
    /// </summary>
    [NotNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public string ExternalUserId { get; }
    /// <summary>
    /// 要标记的标签列表
    /// </summary>
    [NotNull]
    [JsonProperty("add_tag")]
    [JsonPropertyName("add_tag")]
    public List<string> CreateTag { get; }
    /// <summary>
    /// 要移除的标签列表
    /// </summary>
    [NotNull]
    [JsonProperty("remove_tag")]
    [JsonPropertyName("remove_tag")]
    public List<string> RemoveTag { get; }
    public WeChatWorkMarkCropTagRequest(string userId, string externalUserId)
    {
        Check.NotNullOrWhiteSpace(userId, nameof(userId));
        Check.NotNullOrWhiteSpace(externalUserId, nameof(externalUserId));

        UserId = userId;
        ExternalUserId = externalUserId;

        CreateTag = new List<string>();
        RemoveTag = new List<string>();
    }

    protected override void Validate()
    {
        if (CreateTag.IsNullOrEmpty() &&
            RemoveTag.IsNullOrEmpty())
        {
            throw new ArgumentException("CreateTag and RemoveTag cannot be empty simultaneously!");
        }
    }
}
