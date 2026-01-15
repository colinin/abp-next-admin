using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Contacts.Tags.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Tags.Response;
/// <summary>
/// 获取标签列表响应参数
/// </summary>
/// <remarks>
/// 详情见: https://developer.work.weixin.qq.com/document/path/90216
/// </remarks>
public class WeChatWorkTagListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 标签列表
    /// </summary>
    [NotNull]
    [JsonProperty("taglist")]
    [JsonPropertyName("taglist")]
    public TagInfo[] Tags { get; set; }
}