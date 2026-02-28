using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Response;
/// <summary>
/// 添加企业客户标签响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92117#%E6%B7%BB%E5%8A%A0%E4%BC%81%E4%B8%9A%E5%AE%A2%E6%88%B7%E6%A0%87%E7%AD%BE" />
/// </remarks>
public class WeChatWorkCreateCropTagResponse : WeChatWorkResponse
{
    /// <summary>
    /// 标签组
    /// </summary>
    [NotNull]
    [JsonProperty("tag_group")]
    [JsonPropertyName("tag_group")]
    public CropTagGroup TagGroup { get; set; }
}
