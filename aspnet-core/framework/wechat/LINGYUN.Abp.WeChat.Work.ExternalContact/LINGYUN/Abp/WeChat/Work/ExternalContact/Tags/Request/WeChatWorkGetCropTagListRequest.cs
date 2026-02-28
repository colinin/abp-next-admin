using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Request;
/// <summary>
/// 获取企业标签库请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92117#%E8%8E%B7%E5%8F%96%E4%BC%81%E4%B8%9A%E6%A0%87%E7%AD%BE%E5%BA%93" />
/// </remarks>
public class WeChatWorkGetCropTagListRequest : WeChatWorkRequest
{
    /// <summary>
    /// 要查询的标签id
    /// </summary>
    [CanBeNull]
    [JsonProperty("tag_id")]
    [JsonPropertyName("tag_id")]
    public string[]? TagId { get; }
    /// <summary>
    /// 要查询的标签组id，返回该标签组以及其下的所有标签信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("group_id")]
    [JsonPropertyName("group_id")]
    public string[]? GroupId { get; }
    public WeChatWorkGetCropTagListRequest(string[]? tagId = null, string[]? groupId = null)
    {
        TagId = tagId;
        GroupId = groupId;
    }
}
