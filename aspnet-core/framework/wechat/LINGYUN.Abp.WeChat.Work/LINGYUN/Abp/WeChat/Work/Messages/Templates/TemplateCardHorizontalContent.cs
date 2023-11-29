using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 二级标题+文本
/// </summary>
public class TemplateCardHorizontalContent
{
    public static TemplateCardHorizontalContent None(string keyName, string value = "")
    {
        return new TemplateCardHorizontalContent(
            keyName, 0, value);
    }

    public static TemplateCardHorizontalContent FromUrl(string keyName, string url, string value = "")
    {
        return new TemplateCardHorizontalContent(
            keyName, 1, value, url);
    }

    public static TemplateCardHorizontalContent FromMedia(string keyName, string mediaId, string value = "")
    {
        return new TemplateCardHorizontalContent(
            keyName, 2, value, mediaId: mediaId);
    }

    public static TemplateCardHorizontalContent FromUser(string keyName, string userId, string value = "")
    {
        return new TemplateCardHorizontalContent(
            keyName, 3, value, userId: userId);
    }

    public TemplateCardHorizontalContent(
        string keyName,
        byte type = 0, 
        string value = "", 
        string url = "", 
        string mediaId = "", 
        string userId = "")
    {
        Type = type;
        KeyName = keyName;
        Value = value;
        Url = url;
        MediaId = mediaId;
        UserId = userId;
    }

    /// <summary>
    /// 链接类型，0或不填代表不是链接，1 代表跳转url，2 代表下载附件，3 代表点击跳转成员详情
    /// </summary>
    [CanBeNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public byte Type { get; }
    /// <summary>
    /// 二级标题，建议不超过5个字
    /// </summary>
    [NotNull]
    [JsonProperty("keyname")]
    [JsonPropertyName("keyname")]
    public string KeyName { get; set; }
    /// <summary>
    /// 二级文本，如果horizontal_content_list.type是2，该字段代表文件名称（要包含文件类型），建议不超过30个字，（支持id转译）
    /// </summary>
    [CanBeNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public string Value { get; set; }
    /// <summary>
    /// 链接跳转的url，horizontal_content_list.type是1时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 附件的media_id，horizontal_content_list.type是2时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
    /// <summary>
    /// 成员详情的userid，horizontal_content_list.type是3时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
}
