using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Common.Members.Models;
/// <summary>
/// 视频号属性
/// </summary>
public class WechatChannel
{
    /// <summary>
    /// 视频号名字（设置后，成员将对外展示该视频号）
    /// </summary>
    [NotNull]
    [JsonProperty("nickname")]
    [JsonPropertyName("nickname")]
    public string NickName { get; set; }
    /// <summary>
    /// 对外展示视频号状态
    /// </summary>
    [NotNull]
    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public WechatChannelStatus Status { get; set; }
}
