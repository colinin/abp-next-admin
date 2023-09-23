using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Chat.Request;
public class WeChatWorkAppChatCreateRequest : WeChatWorkAppChatRequest
{
    public WeChatWorkAppChatCreateRequest(
        string agentId,
        string name,
        List<string> users,
        string owner = null,
        string chatId = null)
        : base(agentId)
    {
        Name = name;
        Owner = owner;
        Users = users;
        ChatId = chatId;
    }

    /// <summary>
    /// 群聊名，最多50个utf8字符，超过将截断
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public virtual string Name { get; set; }
    /// <summary>
    /// 指定群主的id。
    /// 如果不指定，系统会随机从userlist中选一人作为群主
    /// </summary>
    [CanBeNull]
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public virtual string Owner { get; set; }
    /// <summary>
    /// 群成员id列表。
    /// 至少2人，至多2000人
    /// </summary>
    [NotNull]
    [JsonProperty("userlist")]
    [JsonPropertyName("userlist")]
    public virtual List<string> Users { get; set; }
    /// <summary>
    /// 群聊的唯一标志，不能与已有的群重复；
    /// 字符串类型，最长32个字符。
    /// 只允许字符0-9及字母a-zA-Z。
    /// 如果不填，系统会随机生成群id
    /// </summary>
    [CanBeNull]
    [JsonProperty("chatid")]
    [JsonPropertyName("chatid")]
    public virtual string ChatId { get; set; }
}
