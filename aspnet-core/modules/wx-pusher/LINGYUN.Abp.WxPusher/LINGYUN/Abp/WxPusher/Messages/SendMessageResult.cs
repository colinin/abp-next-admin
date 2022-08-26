using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.WxPusher.Messages;

[Serializable]
public class SendMessageResult
{
    /// <summary>
    /// 状态码
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }
    /// <summary>
    /// 消息标识
    /// </summary>
    [JsonProperty("messageId")]
    public long MessageId { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }
    /// <summary>
    /// 用户标识
    /// </summary>
    [JsonProperty("uid")]
    public string Uid { get; set; }
    /// <summary>
    /// 群组标识
    /// </summary>
    [JsonProperty("topicId")]
    public string TopicId { get; set; }
    /// <summary>
    /// 是否调用成功
    /// </summary>
    public bool IsSuccessed => Code == 1000;
}
