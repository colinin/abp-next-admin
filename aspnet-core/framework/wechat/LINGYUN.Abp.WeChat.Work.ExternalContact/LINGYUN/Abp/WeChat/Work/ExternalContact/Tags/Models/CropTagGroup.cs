using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Models;
public class CropTagGroup : TagGroup
{
    /// <summary>
    /// 标签组是否已经被删除，只在指定tag_id进行查询时返回
    /// </summary>
    [NotNull]
    [JsonProperty("deleted")]
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
    /// <summary>
    /// 标签列表
    /// </summary>
    [NotNull]
    [JsonProperty("tag")]
    [JsonPropertyName("tag")]
    public CropTag[] Tag { get; set; }
}
