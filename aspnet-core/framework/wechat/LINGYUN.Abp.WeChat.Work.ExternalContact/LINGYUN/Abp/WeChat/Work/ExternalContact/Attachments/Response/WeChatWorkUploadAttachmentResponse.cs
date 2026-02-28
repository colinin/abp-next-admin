using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Response;
/// <summary>
/// 上传附件资源响应结果
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95098" />
/// </remarks>
public class WeChatWorkUploadAttachmentResponse : WeChatWorkResponse
{
    /// <summary>
    /// 媒体文件类型，分别有图片（image）、语音（voice）、视频（video），普通文件(file)
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string MediaType { get; set; }
    /// <summary>
    /// 媒体文件上传后获取的唯一标识，三天有效，可使用获取临时素材接口获取
    /// </summary>
    [NotNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
    /// <summary>
    /// 媒体文件上传时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("created_at")]
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }
}
