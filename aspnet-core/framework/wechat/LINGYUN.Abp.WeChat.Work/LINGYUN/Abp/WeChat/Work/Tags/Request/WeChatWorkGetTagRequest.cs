using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Tags.Request;
/// <summary>
/// 获取标签请求参数
/// </summary>
public class WeChatWorkGetTagRequest
{
    /// <summary>
    /// 标签id
    /// </summary>
    [NotNull]
    [JsonProperty("tagid")]
    [JsonPropertyName("tagid")]
    public int TagId { get; set; }
    public WeChatWorkGetTagRequest(int tagId)
    {
        TagId = Check.Positive(tagId, nameof(tagId));
    }
}
