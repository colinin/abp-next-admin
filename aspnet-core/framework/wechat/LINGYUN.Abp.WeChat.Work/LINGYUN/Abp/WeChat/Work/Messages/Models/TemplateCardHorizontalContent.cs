using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 二级标题+文本
/// </summary>
public class TemplateCardHorizontalContent
{
    /// <summary>
    /// 模版卡片的二级标题信息内容支持的类型，1是url，2是文件附件，3 代表点击跳转成员详情
    /// </summary>
    [CanBeNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public int? Type { get; set; }
    /// <summary>
    /// 二级标题，建议不超过5个字
    /// </summary>
    [NotNull]
    [JsonProperty("keyname")]
    [JsonPropertyName("keyname")]
    public string KeyName { get; set; }
    /// <summary>
    /// 二级文本，如果type是2，该字段代表文件名称（要包含文件类型），建议不超过26个字
    /// </summary>
    [CanBeNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public string Value { get; set; }
    /// <summary>
    /// 链接跳转的url，type是1时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
    /// <summary>
    /// 附件的media_id，type是2时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("media_id")]
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
    /// <summary>
    /// 成员详情的userid，type是3时必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    private TemplateCardHorizontalContent(
        string keyName,
        int? type = null,
        string value = null,
        string url = null,
        string mediaId = null,
        string userId = null)
    {
        Type = type;
        KeyName = keyName;
        Value = value;
        Url = url;
        MediaId = mediaId;
        UserId = userId;
    }
    /// <summary>
    /// 创建一个跳转链接的二级标题+文本
    /// </summary>
    /// <param name="keyName">二级标题</param>
    /// <param name="url">链接跳转的url</param>
    /// <param name="value">二级文本</param>
    /// <returns></returns>
    public static TemplateCardHorizontalContent Link(string keyName, string url, string value = null)
    {
        Check.NotNullOrWhiteSpace(keyName, nameof(keyName));
        Check.NotNullOrWhiteSpace(url, nameof(url));

        return new TemplateCardHorizontalContent(keyName, 1, value: value, url: url);
    }
    /// <summary>
    /// 创建一个引用文件的二级标题+文本
    /// </summary>
    /// <param name="keyName">二级标题</param>
    /// <param name="fileName">文件名称</param>
    /// <param name="mediaId">附件的mediaId</param>
    /// <returns></returns>
    public static TemplateCardHorizontalContent File(string keyName, string fileName, string mediaId)
    {
        Check.NotNullOrWhiteSpace(keyName, nameof(keyName));
        Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
        Check.NotNullOrWhiteSpace(mediaId, nameof(mediaId));

        return new TemplateCardHorizontalContent(keyName, 2, value: fileName, mediaId: mediaId);
    }
    /// <summary>
    /// 创建一个成员详情的二级标题+文本
    /// </summary>
    /// <param name="keyName">二级标题</param>
    /// <param name="userId">成员的userid</param>
    /// <returns></returns>
    public static TemplateCardHorizontalContent UserInfo(string keyName, string userId)
    {
        Check.NotNullOrWhiteSpace(keyName, nameof(keyName));
        Check.NotNullOrWhiteSpace(userId, nameof(userId));

        return new TemplateCardHorizontalContent(keyName, 3, userId: userId);
    }
}
