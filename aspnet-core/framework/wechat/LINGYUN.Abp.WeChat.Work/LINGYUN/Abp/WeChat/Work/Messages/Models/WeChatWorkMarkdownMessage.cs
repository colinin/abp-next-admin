using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信markdown消息
/// </summary>
public class WeChatWorkMarkdownMessage : WeChatWorkMessage
{
    public WeChatWorkMarkdownMessage(
        string agentId,
        MarkdownMessage markdown) : base(agentId, "markdown")
    {
        Markdown = markdown;
    }
    /// <summary>
    /// markdown消息
    /// </summary>
    [NotNull]
    [JsonProperty("markdown")]
    [JsonPropertyName("markdown")]
    public MarkdownMessage Markdown { get; set; }
    /// <summary>
    /// 表示是否开启重复消息检查，0表示否，1表示是，默认0
    /// </summary>
    [JsonProperty("enable_duplicate_check")]
    [JsonPropertyName("enable_duplicate_check")]
    public byte EnableDuplicateCheck { get; set; }
    /// <summary>
    /// 表示是否重复消息检查的时间间隔，默认1800s，最大不超过4小时
    /// </summary>
    [JsonProperty("duplicate_check_interval")]
    [JsonPropertyName("duplicate_check_interval")]
    public int DuplicateCheckInterval { get; set; } = 1800;
}
