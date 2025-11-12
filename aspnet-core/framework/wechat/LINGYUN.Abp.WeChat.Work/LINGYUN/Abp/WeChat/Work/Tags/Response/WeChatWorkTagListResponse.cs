using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Tags.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Tags.Response;
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
    public List<TagInfo> Tags { get; set; }
}