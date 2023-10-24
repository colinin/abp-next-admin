using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.PushPlus.Channel.Webhook;
/// <summary>
/// webhook
/// </summary>
public class PushPlusWebhook
{
    /// <summary>
    /// webhook编号
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
    /// <summary>
    /// webhook编码
    /// </summary>
    [JsonProperty("webhookCode")]
    public string WebhookCode { get; set; }
    /// <summary>
    /// webhook名称
    /// </summary>
    [JsonProperty("webhookName")]
    public string WebhookName { get; set; }
    /// <summary>
    /// webhook类型；
    /// 1-企业微信，
    /// 2-钉钉，
    /// 3-飞书，
    /// 4-server酱
    /// </summary>
    [JsonProperty("webhookType")]
    public PushPlusWebhookType WebhookType { get; set; }
    /// <summary>
    /// 调用的url地址
    /// </summary>
    [JsonProperty("webhookUrl")]
    public string WebhookUrl { get; set; }
    /// <summary>
    /// 创建日期
    /// </summary>
    [JsonProperty("createTime")]
    public DateTime CreateTime { get; set; }
}
