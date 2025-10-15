using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook图片消息体
/// </summary>
public class WebhookImageMessage
{
    /// <summary>
    /// 图片内容的base64编码
    /// </summary>
    [NotNull]
    [JsonProperty("base64")]
    [JsonPropertyName("base64")]
    public string Base64 { get; set; }
    /// <summary>
    /// 图片的md5
    /// </summary>
    [NotNull]
    [JsonProperty("md5")]
    [JsonPropertyName("md5")]
    public string Md5 { get; set; }
    /// <summary>
    /// 创建一个Webhook图片消息体
    /// </summary>
    /// <param name="base64">图片内容的base64编码</param>
    /// <param name="md5">图片的md5</param>
    public WebhookImageMessage(string base64, string md5)
    {
        Base64 = base64;
        Md5 = md5;
    }
}
