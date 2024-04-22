using Newtonsoft.Json;

namespace LINGYUN.Abp.WeChat.Work.Media.Models;
public class WeChatWorkMediaResponse : WeChatWorkResponse
{
    /// <summary>
    /// 媒体文件类型
    /// </summary>
    /// <remarks>
    /// 图片（image）
    /// 语音（voice）
    /// 视频（video）
    /// 普通文件(file)
    /// </remarks>
    [JsonProperty("type")]
    public string Type { get; set; }
    /// <summary>
    /// 媒体文件上传后获取的唯一标识，3天内有效
    /// </summary>
    [JsonProperty("media_id")]
    public string MediaId { get; set; }
    /// <summary>
    /// 媒体文件上传时间戳
    /// </summary>
    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }
}
