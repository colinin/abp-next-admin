using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 企业微信文本图文消息
/// </summary>
public class WeChatWorkMpNewMessage : WeChatWorkMessage
{
    public WeChatWorkMpNewMessage(
        string agentId,
        MpNewMessagePayload mpnews) : base(agentId, "mpnews")
    {
        News = mpnews;
    }
    /// <summary>
    /// 图文消息（mp）
    /// </summary>
    [NotNull]
    [JsonProperty("mpnews")]
    [JsonPropertyName("mpnews")]
    public MpNewMessagePayload News { get; set; }
    /// <summary>
    /// 表示是否是保密消息，
    /// 0表示可对外分享，
    /// 1表示不能分享且内容显示水印，
    /// 2表示仅限在企业内分享，默认为0；
    /// 注意仅mpnews类型的消息支持safe值为2，其他消息类型不支持
    /// </summary>
    [JsonProperty("safe")]
    [JsonPropertyName("safe")]
    public int Safe { get; set; }
    /// <summary>
    /// 表示是否开启id转译，0表示否，1表示是，默认0。
    /// 仅第三方应用需要用到
    /// 企业自建应用可以忽略。
    /// </summary>
    [JsonProperty("enable_id_trans")]
    [JsonPropertyName("enable_id_trans")]
    public byte EnableIdTrans { get; set; }
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
