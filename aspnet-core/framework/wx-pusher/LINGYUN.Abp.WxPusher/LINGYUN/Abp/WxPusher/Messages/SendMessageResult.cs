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
    [Obsolete("废弃，请不要再使用，后续会删除这个字段, see: https://wxpusher.dingliqc.com/docs/#/?id=%e5%8f%91%e9%80%81%e6%b6%88%e6%81%af")]
    public long? MessageId { get; set; }
    /// <summary>
    /// 消息内容标识
    /// </summary>
    /// <remarks>
    /// 调用一次接口，生成一个，你可以通过此id调用删除消息接口，删除消息。
    /// 本次发送的所有用户共享此消息内容。
    /// </remarks>
    [JsonProperty("messageContentId")]
    public long ContentId { get; set; }
    /// <summary>
    /// 消息发送标识
    /// </summary>
    /// <remarks>
    /// 每个uid用户或者topicId生成一个，可以通过这个id查询对某个用户的发送状态
    /// </remarks>
    [JsonProperty("sendRecordId")]
    public long RecordId { get; set; }
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
    public long? TopicId { get; set; }
    /// <summary>
    /// 是否调用成功
    /// </summary>
    public bool IsSuccessed => Code == 1000;
}
