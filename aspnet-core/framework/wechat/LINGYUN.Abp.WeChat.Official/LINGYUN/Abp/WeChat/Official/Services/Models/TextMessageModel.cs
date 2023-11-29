using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Official.Services.Models;
/// <summary>
/// 发送文本消息
/// </summary>
[Serializable]
public class TextMessageModel : MessageModel
{
    /// <summary>
    /// 接收消息用户
    /// </summary>
    [JsonProperty("touser")]
    [JsonPropertyName("touser")]
    public string ToUser { get; }
    /// <summary>
    /// 消息内容
    /// </summary>
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public TextMessage Text { get; }
    public TextMessageModel(string toUser, TextMessage text)
        : base("text")
    {
        ToUser = toUser;
        Text = text;
    }
}

public class TextMessage
{
    /// <summary>
    /// 内容文本
    /// </summary>
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }
    public TextMessage(string content)
    {
        Content = content;
    }
}
