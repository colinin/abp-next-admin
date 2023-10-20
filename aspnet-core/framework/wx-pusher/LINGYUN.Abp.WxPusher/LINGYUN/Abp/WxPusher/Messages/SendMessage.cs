using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.WxPusher.Messages;

[Serializable]
public class SendMessage
{
    [JsonProperty("appToken")]
    public string AppToken { get; }

    [JsonProperty("content")]
    public string Content { get; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("contentType")]
    public MessageContentType ContentType { get; }

    [JsonProperty("topicIds")]
    public List<int> TopicIds { get; }

    [JsonProperty("uids")]
    public List<string> Uids { get; }

    [JsonProperty("url")]
    public string Url { get; }
    public SendMessage(
        [NotNull] string appToken,
        [NotNull] string content,
        string summary = "",
        MessageContentType contentType = MessageContentType.Text,
        string url = "")
    {
        Check.NotNullOrWhiteSpace(appToken, nameof(appToken));
        Check.NotNullOrWhiteSpace(content, nameof(content));
        Check.Length(summary, nameof(summary), 100);

        AppToken = appToken;
        Content = content;
        Summary = summary;
        ContentType = contentType;
        Url = url;

        TopicIds = new List<int>();
        Uids = new List<string>();
    }
}
