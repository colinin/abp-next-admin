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
        AppToken = Check.NotNullOrWhiteSpace(appToken, nameof(appToken));
        // 单条消息的数据长度(字符数)限制是：content<40000;summary<20(微信的限制，大于20显示不完);url<400
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), 39999);
        Summary = Check.Length(summary, nameof(summary), 19);
        Url = Check.Length(url, nameof(url), 399);
        ContentType = contentType;

        TopicIds = new List<int>();
        Uids = new List<string>();
    }
}
