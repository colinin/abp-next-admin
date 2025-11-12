using JetBrains.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Tags.Request;
/// <summary>
/// 创建标签请求参数
/// </summary>
public class WeChatWorkTagCreateRequest
{
    /// <summary>
    /// 标签id，非负整型，指定此参数时新增的标签会生成对应的标签id，不指定时则以目前最大的id自增。
    /// </summary>
    [NotNull]
    [JsonProperty("tagid")]
    [JsonPropertyName("tagid")]
    public int? TagId { get; set; }
    /// <summary>
    /// 标签名称，长度限制为32个字以内（汉字或英文字母），标签名不可与其他标签重名。
    /// </summary>
    [NotNull]
    [StringLength(32)]
    [JsonProperty("tagname")]
    [JsonPropertyName("tagname")]
    public string TagName { get; set; }
    public WeChatWorkTagCreateRequest(string tagName, int? tagId = null)
    {
        TagName = Check.NotNullOrWhiteSpace(tagName, nameof(tagName), 32);
        TagId = tagId;
    }
}
