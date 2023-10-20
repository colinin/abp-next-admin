using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Chat.Response;

public class WeChatWorkAppChatCreateResponse : WeChatWorkResponse
{
    /// <summary>
    /// 群聊的唯一标志
    /// </summary>
    [JsonProperty("chatid")]
    [JsonPropertyName("chatid")]
    public virtual string ChatId { get; set; }
}
