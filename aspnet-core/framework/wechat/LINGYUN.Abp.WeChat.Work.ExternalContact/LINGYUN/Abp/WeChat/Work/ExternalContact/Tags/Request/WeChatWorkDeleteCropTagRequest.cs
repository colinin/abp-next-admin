using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Request;
/// <summary>
/// 删除企业客户标签请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92117#%E5%88%A0%E9%99%A4%E4%BC%81%E4%B8%9A%E5%AE%A2%E6%88%B7%E6%A0%87%E7%AD%BE" />
/// </remarks>
public class WeChatWorkDeleteCropTagRequest : WeChatWorkRequest
{
    /// <summary>
    /// 标签组的id列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("group_id")]
    [JsonPropertyName("group_id")]
    public string[]? GroupId { get; }
    /// <summary>
    /// 标签组的id列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("tag_id")]
    [JsonPropertyName("tag_id")]
    public string[]? TagId { get; }
    /// <summary>
    /// 授权方安装的应用agentid。仅旧的第三方多应用套件需要填此参数
    /// </summary>
    [CanBeNull]
    [JsonProperty("agentid")]
    [JsonPropertyName("agentid")]
    public string? AgentId { get; set; }

    private WeChatWorkDeleteCropTagRequest(
        string[]? groupId = null,
        string[]? tagId = null)
    {
        GroupId = groupId;
        TagId = tagId;
    }

    public static WeChatWorkDeleteCropTagRequest Tag(string[] tagId)
    {
        return new WeChatWorkDeleteCropTagRequest(tagId: tagId);
    }

    public static WeChatWorkDeleteCropTagRequest Group(string[] groupId)
    {
        return new WeChatWorkDeleteCropTagRequest(groupId);
    }
}
