using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Chat.Models;
public class WeChatWorkAppChatInfo
{
    /// <summary>
    /// 群聊名
    /// </summary>
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public virtual string Name { get; set; }
    /// <summary>
    /// 群主id
    /// </summary>
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public virtual string Owner { get; set; }
    /// <summary>
    /// 群成员id列表
    /// </summary>
    [JsonProperty("userlist")]
    [JsonPropertyName("userlist")]
    public virtual List<string> Users { get; set; }
    /// <summary>
    /// 群聊唯一标志
    /// </summary>
    [JsonProperty("chatid")]
    [JsonPropertyName("chatid")]
    public virtual string ChatId { get; set; }
}
