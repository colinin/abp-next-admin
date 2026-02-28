using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 视频号信息
/// </summary>
public class FollowUserWechatChannel
{
    /// <summary>
    /// 视频号名称
    /// </summary>
    [NotNull]
    [JsonProperty("nickname")]
    [JsonPropertyName("nickname")]
    public string NickName { get; set; }
    /// <summary>
    /// 视频号添加场景
    /// </summary>
    [NotNull]
    [JsonProperty("source")]
    [JsonPropertyName("source")]
    public FollowUserWechatChannelSource Source { get; set; }
}
