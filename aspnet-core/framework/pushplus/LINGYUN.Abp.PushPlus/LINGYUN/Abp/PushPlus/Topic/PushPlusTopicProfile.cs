using Newtonsoft.Json;

namespace LINGYUN.Abp.PushPlus.Topic;
/// <summary>
/// 群组详情
/// </summary>
public class PushPlusTopicProfile : PushPlusTopic
{
    /// <summary>
    /// 永久二维码图片地址
    /// </summary>
    [JsonProperty("qrCodeImgUrl")]
    public string QrCodeImgUrl { get; set; }
    /// <summary>
    /// 联系方式
    /// </summary>
    [JsonProperty("contact")]
    public string Contact { get; set; }
    /// <summary>
    /// 群组简介
    /// </summary>
    [JsonProperty("introduction")]
    public string Introduction { get; set; }
    /// <summary>
    /// 加入后回复内容
    /// </summary>
    [JsonProperty("receiptMessage")]
    public string ReceiptMessage { get; set; }
    /// <summary>
    /// 群组订阅人总数
    /// </summary>
    [JsonProperty("topicUserCount")]
    public string TopicUserCount { get; set; }
}
