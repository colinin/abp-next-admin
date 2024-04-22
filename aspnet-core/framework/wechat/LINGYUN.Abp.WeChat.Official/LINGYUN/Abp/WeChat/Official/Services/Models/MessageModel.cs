using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Official.Services.Models;
public abstract class MessageModel : WeChatRequest
{
    /// <summary>
    /// 消息类型
    /// </summary>
    [JsonProperty("msgtype")]
    [JsonPropertyName("msgtype")]
    public string MsgType { get; }
    protected MessageModel(string msgType)
    {
        MsgType = msgType;
    }
}
