using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.PushPlus.Message;

public class PushPlusMessage
{
    /// <summary>
    /// 消息发送渠道；
    /// wechat-微信公众号,
    /// mail-邮件,
    /// cp-企业微信应用,
    /// webhook-第三方webhook
    /// </summary>
    [JsonProperty("channel")]
    public string Channel { get; set; }
    /// <summary>
    /// 消息类型;
    /// 1-一对一消息,
    /// 2-一对多消息
    /// </summary>
    [JsonProperty("messageType")]
    public PushPlusMessageType MessageType { get; set; }
    /// <summary>
    /// 消息短链码;可用于查询消息发送结果
    /// </summary>
    [JsonProperty("shortCode")]
    public string ShortCode { get; set; }
    /// <summary>
    /// 消息标题
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; }
    /// <summary>
    /// 群组名称，一对多消息才有值
    /// </summary>
    [JsonProperty("topicName")]
    public string TopicName { get; set; }
    /// <summary>
    /// 更新日期
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime UpdateTime { get; set; }
}
