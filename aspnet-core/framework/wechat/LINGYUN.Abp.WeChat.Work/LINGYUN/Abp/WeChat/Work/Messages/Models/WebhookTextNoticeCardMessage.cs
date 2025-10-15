using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook 文本通知模版卡片消息体
/// </summary>
public class WebhookTextNoticeCardMessage : WebhookTemplateCardMessage
{
    /// <summary>
    /// 二级普通文本，建议不超过112个字。模版卡片主要内容的一级标题main_title.title和二级普通文本sub_title_text必须有一项填写
    /// </summary>
    [CanBeNull]
    [JsonProperty("sub_title_text")]
    [JsonPropertyName("sub_title_text")]
    public string SubTitleText { get; set; }
    /// <summary>
    /// 关键数据样式
    /// </summary>
    [CanBeNull]
    [JsonProperty("emphasis_content")]
    [JsonPropertyName("emphasis_content")]
    public TemplateCardEmphasisContent EmphasisContent { get; set; }
    /// <summary>
    /// 创建一个Webhook 文本通知模版卡片消息体
    /// </summary>
    /// <param name="action">整体卡片的点击跳转事件</param>
    /// <param name="mainTitle">模版卡片的主要内容</param>
    /// <param name="subTitleText">二级普通文本</param>
    /// <param name="emphasisContent">关键数据样式</param>
    /// <param name="source">卡片来源样式信息</param>
    /// <param name="quoteArea">引用文献样式</param>
    /// <param name="horizontalContents">二级标题+文本列表,列表长度不超过6</param>
    /// <param name="jumps">跳转指引样式的列表,列表长度不超过3</param>
    /// <exception cref="ArgumentException"></exception>
    public WebhookTextNoticeCardMessage(
        TemplateCardAction action,
        TemplateCardMainTitle mainTitle = null,
        string subTitleText = null,
        TemplateCardEmphasisContent emphasisContent = null,
        TemplateCardSource source = null,
        TemplateCardQuoteArea quoteArea = null,
        List<TemplateCardHorizontalContent> horizontalContents = null,
        List<TemplateCardJump> jumps = null) 
        : base("text_notice", action, mainTitle, source, quoteArea, horizontalContents, jumps)
    {
        MainTitle = mainTitle;
        SubTitleText = subTitleText;
        EmphasisContent = emphasisContent;
        Source = source;
        QuoteArea = quoteArea;
        HorizontalContents = horizontalContents;
        Jumps = jumps;

        if (mainTitle == null && subTitleText.IsNullOrWhiteSpace())
        {
            throw new ArgumentException("One of the primary title mainTitle and the secondary ordinary text subTitleText of the main content of the template card must be filled in!");
        }
    }
}
