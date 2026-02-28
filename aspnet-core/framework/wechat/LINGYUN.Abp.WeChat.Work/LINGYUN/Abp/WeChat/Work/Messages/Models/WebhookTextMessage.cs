using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook文本消息体
/// </summary>
public class WebhookTextMessage
{
    /// <summary>
    /// 消文本内容，最长不超过2048个字节，必须是utf8编码
    /// </summary>
    [NotNull]
    [StringLength(2048)]
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }
    /// <summary>
    /// userid的列表，提醒群中的指定成员(@某个成员)，@all表示提醒所有人，如果开发者获取不到userid，可以使用mentioned_mobile_list
    /// </summary>
    [NotNull]
    [JsonProperty("mentioned_list")]
    [JsonPropertyName("mentioned_list")]
    public List<string> MentionedList { get; set; }
    /// <summary>
    /// 手机号列表，提醒手机号对应的群成员(@某个成员)，@all表示提醒所有人
    /// </summary>
    [NotNull]
    [StringLength(2048)]
    [JsonProperty("mentioned_mobile_list")]
    [JsonPropertyName("mentioned_mobile_list")]
    public List<string> MentionedMobileList { get; set; }
    /// <summary>
    /// 创建一个Webhook文本消息体
    /// </summary>
    /// <param name="content">消息内容</param>
    public WebhookTextMessage(string content)
    {
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), 2048);
    }
}
