using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 说明文字控件配置
/// </summary>
public class TipsControlConfig : ControlConfig
{
    /// <summary>
    /// 说明文字控件
    /// </summary>
    [NotNull]
    [JsonProperty("tips")]
    [JsonPropertyName("tips")]
    public TipsConfig Tips { get; set; }
    public TipsControlConfig()
    {

    }

    public TipsControlConfig(TipsConfig tips)
    {
        Tips = tips;
    }
}

public class TipsConfig
{
    /// <summary>
    /// 说明文字数组，元素为不同语言的富文本说明文字
    /// </summary>
    [NotNull]
    [JsonProperty("tips_content")]
    [JsonPropertyName("tips_content")]
    public List<TipsContent> TipsContents { get; set; }
    public TipsConfig()
    {

    }

    public TipsConfig(List<TipsContent> tipsContents)
    {
        TipsContents = tipsContents;
    }
}

public class TipsContent
{
    /// <summary>
    /// 某个语言的富文本说明
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public TipsContentText Text { get; set; }
    /// <summary>
    /// 多语言名称
    /// </summary>
    [NotNull]
    [JsonProperty("lang")]
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    public TipsContent()
    {

    }

    public TipsContent(TipsContentText text, string lang = "zh_CN")
    {
        Text = text;
        Lang = lang;
    }
}

public class TipsContentText
{
    /// <summary>
    /// 说明文字分段
    /// </summary>
    [NotNull]
    [JsonProperty("sub_text")]
    [JsonPropertyName("sub_text")]
    public List<TipsContentSubText> SubText { get; set; }
    public TipsContentText()
    {

    }

    public TipsContentText(List<TipsContentSubText> subText)
    {
        SubText = subText;
    }
}

public class TipsContentSubText
{
    /// <summary>
    /// 文本类型 1:纯文本 2:链接，每个说明文字中只支持包含一个链接
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public byte Type { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    [NotNull]
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public SubTextContent Content { get; set; }
    public TipsContentSubText()
    {

    }

    private TipsContentSubText(byte type, SubTextContent content)
    {
        Type = type;
        Content = content;
    }

    public static TipsContentSubText Text(TipsContentPlainText content)
    {
        return new TipsContentSubText(1, content);
    }

    public static TipsContentSubText Link(TipsContentLinkText content)
    {
        return new TipsContentSubText(2, content);
    }
}

public abstract class SubTextContent
{
}

public class TipsContentPlainText : SubTextContent
{
    /// <summary>
    /// 纯文本文字
    /// </summary>
    [NotNull]
    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class TipsContentLinkText : SubTextContent
{
    /// <summary>
    /// 链接标题
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 链接url，不能超过600个字符
    /// </summary>
    [NotNull]
    [StringLength(600)]
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string Url { get; set; }
}