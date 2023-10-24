using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.PushPlus.Topic;
/// <summary>
/// 群组
/// </summary>
public class PushPlusTopic
{
    /// <summary>
    /// 群组编号
    /// </summary>
    [JsonProperty("topicId")]
    public int TopicId { get; set; }
    /// <summary>
    /// 群组编码
    /// </summary>
    [JsonProperty("topicCode")]
    public string TopicCode { get; set; }
    /// <summary>
    /// 群组名称
    /// </summary>
    [JsonProperty("topicName")]
    public string TopicName { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonProperty("createTime")]
    public DateTime CreateTime { get; set; }
}
