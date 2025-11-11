using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Request;
public class WeChatWorkUpdateStrategyTagRequest : WeChatWorkRequest
{
    /// <summary>
    /// 标签或标签组的id
    /// </summary>
    [NotNull]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; }
    /// <summary>
    /// 新的标签或标签组名称，最长为30个字符
    /// </summary>
    [CanBeNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string? Name { get; }
    /// <summary>
    /// 标签/标签组的次序值。order值大的排序靠前。有效的值范围是[0, 2^32)
    /// </summary>
    [CanBeNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int? Order { get; set; }

    public WeChatWorkUpdateStrategyTagRequest(string id, string? name = null)
    {
        Check.NotNullOrWhiteSpace(id, nameof(id));
        Check.Length(name, nameof(name), 30);

        Id = id;
        Name = name;
    }
}
