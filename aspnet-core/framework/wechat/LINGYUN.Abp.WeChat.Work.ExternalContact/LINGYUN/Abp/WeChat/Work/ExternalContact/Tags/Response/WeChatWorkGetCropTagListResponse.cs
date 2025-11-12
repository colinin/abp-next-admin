using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Response;
/// <summary>
/// 获取企业标签库响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92117#%E8%8E%B7%E5%8F%96%E4%BC%81%E4%B8%9A%E6%A0%87%E7%AD%BE%E5%BA%93" />
/// </remarks>
public class WeChatWorkGetCropTagListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 标签组列表
    /// </summary>
    [NotNull]
    [JsonProperty("tag_group")]
    [JsonPropertyName("tag_group")]
    public StrategyTagGroup[] TagGroup { get; set; }
}
