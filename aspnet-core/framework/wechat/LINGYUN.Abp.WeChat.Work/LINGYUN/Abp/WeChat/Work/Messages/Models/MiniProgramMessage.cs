using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// 小程序通知消息
/// </summary>
public class MiniProgramMessage
{
    /// <summary>
    /// 小程序appid，必须是与当前应用关联的小程序
    /// </summary>
    [NotNull]
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }
    /// <summary>
    /// 消息标题，长度限制4-12个汉字（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 点击消息卡片后的小程序页面，最长1024个字节，仅限本小程序内的页面。该字段不填则消息点击后不跳转。
    /// </summary>
    [CanBeNull]
    [JsonProperty("page")]
    [JsonPropertyName("page")]
    public string Page { get; set; }
    /// <summary>
    /// 消息描述，长度限制4-12个汉字（支持id转译）
    /// </summary>
    [CanBeNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string Description { get; set; }
    /// <summary>
    /// 是否放大第一个content_item
    /// </summary>
    [CanBeNull]
    [JsonProperty("emphasis_first_item")]
    [JsonPropertyName("emphasis_first_item")]
    public bool? EmphasisFirstItem { get; set; }
    /// <summary>
    /// 消息内容键值对，最多允许10个item
    /// </summary>
    [CanBeNull]
    [JsonProperty("content_item")]
    [JsonPropertyName("content_item")]
    public List<MiniProgramContent> ContentItems{ get; set; }
    public MiniProgramMessage()
    {
        ContentItems = new List<MiniProgramContent>();
    }

    public MiniProgramMessage(
        string appId,
        string title,
        string page = null, 
        string description = null, 
        bool? emphasisFirstItem = null)
    {
        AppId = appId;
        Page = page;
        Title = title;
        Description = description;
        EmphasisFirstItem = emphasisFirstItem;
    }
}

public class MiniProgramContent
{
    /// <summary>
    /// 长度10个汉字以内
    /// </summary>
    [NotNull]
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    /// <summary>
    /// 长度30个汉字以内（支持id转译）
    /// </summary>
    [NotNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public string Value { get; set; }
}
