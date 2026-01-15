using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Tags.Response;
/// <summary>
/// 创建标签响应参数
/// </summary>
/// <remarks>
/// 详情见: https://developer.work.weixin.qq.com/document/path/90210
/// </remarks>
public class WeChatWorkTagCreateResponse : WeChatWorkResponse
{
    /// <summary>
    /// 标签id
    /// </summary>
    [NotNull]
    [JsonProperty("tagid")]
    [JsonPropertyName("tagid")]
    public int TagId { get; set; }
}
