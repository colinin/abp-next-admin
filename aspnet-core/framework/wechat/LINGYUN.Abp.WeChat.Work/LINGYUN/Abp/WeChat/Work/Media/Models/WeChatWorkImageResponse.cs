using Newtonsoft.Json;

namespace LINGYUN.Abp.WeChat.Work.Media.Models;
public class WeChatWorkImageResponse : WeChatWorkResponse
{
    /// <summary>
    /// 上传后得到的图片URL。永久有效
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; }
}
