using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Chat.Request;
public class WeChatWorkAppChatUpdateRequest : WeChatWorkAppChatRequest
{
    public WeChatWorkAppChatUpdateRequest(
        string agentId,
        string chatId,
        string name = null,
        string owner = null,
        List<string> addUsers = null,
        List<string> delUsers = null)
        : base(agentId)
    {
        Name = name;
        Owner = owner;
        AddUsers = addUsers;
        DelUsers = delUsers;
        ChatId = chatId;
    }

    /// <summary>
    /// 新的群聊名。若不需更新，请忽略此参数。最多50个utf8字符，超过将截断
    /// </summary>
    [CanBeNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public virtual string Name { get; set; }
    /// <summary>
    /// 新群主的id。
    /// 若不需更新，请忽略此参数。课程群聊群主必须在设置的群主列表内
    /// </summary>
    [CanBeNull]
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public virtual string Owner { get; set; }
    /// <summary>
    /// 添加成员的id列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("add_user_list")]
    [JsonPropertyName("add_user_list")]
    public virtual List<string> AddUsers { get; set; }
    /// <summary>
    /// 踢出成员的id列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("del_user_list")]
    [JsonPropertyName("del_user_list")]
    public virtual List<string> DelUsers { get; set; }
    /// <summary>
    /// 群聊id
    /// </summary>
    [NotNull]
    [JsonProperty("chatid")]
    [JsonPropertyName("chatid")]
    public virtual string ChatId { get; set; }
}
