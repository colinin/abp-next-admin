using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 文本卡片消息
/// </summary>
public class TextCardMessage
{
    public TextCardMessage(
        string title,
        string description,
        string url,
        string buttonText = "")
    {
        Title = title;
        Description = description;
        Url = url;
        ButtonText = buttonText;
    }

    /// <summary>
    /// 标题，不超过128个字节，超过会自动截断（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 描述，不超过512个字节，超过会自动截断（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string Description { get; set; }
    /// <summary>
    /// 点击后跳转的链接。最长2048字节，请确保包含了协议头(http/https)
    /// </summary>
    [NotNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 按钮文字。 默认为“详情”， 不超过4个文字，超过自动截断。
    /// </summary>
    [CanBeNull]
    [JsonProperty("btntxt")]
    [JsonPropertyName("btntxt")]
    public string ButtonText { get; set; }
}
