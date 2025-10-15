using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook模板卡片消息体
/// </summary>
public abstract class WebhookTemplateCardMessage
{
    /// <summary>
    /// 模版卡片的模版类型
    /// </summary>
    [CanBeNull]
    [JsonProperty("card_type")]
    [JsonPropertyName("card_type")]
    public string CardType { get; set; }
    /// <summary>
    /// 卡片来源样式信息，不需要来源样式可不填写
    /// </summary>
    [CanBeNull]
    [JsonProperty("source")]
    [JsonPropertyName("source")]
    public TemplateCardSource Source { get; set; }
    /// <summary>
    /// 模版卡片的主要内容，包括一级标题和标题辅助信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("main_title")]
    [JsonPropertyName("main_title")]
    public TemplateCardMainTitle MainTitle { get; set; }
    /// <summary>
    /// 引用文献样式，建议不与关键数据共用
    /// </summary>
    [CanBeNull]
    [JsonProperty("quote_area")]
    [JsonPropertyName("quote_area")]
    public TemplateCardQuoteArea QuoteArea { get; set; }
    /// <summary>
    /// 二级标题+文本列表，该字段可为空数组，但有数据的话需确认对应字段是否必填，列表长度不超过6
    /// </summary>
    [CanBeNull]
    [JsonProperty("horizontal_content_list")]
    [JsonPropertyName("horizontal_content_list")]
    public List<TemplateCardHorizontalContent> HorizontalContents { get; set; }
    /// <summary>
    /// 跳转指引样式的列表，该字段可为空数组，但有数据的话需确认对应字段是否必填，列表长度不超过3
    /// </summary>
    [CanBeNull]
    [JsonProperty("jump_list")]
    [JsonPropertyName("jump_list")]
    public List<TemplateCardJump> Jumps { get; set; }
    /// <summary>
    /// 整体卡片的点击跳转事件，text_notice模版卡片中该字段为必填项
    /// </summary>
    [CanBeNull]
    [JsonProperty("card_action")]
    [JsonPropertyName("card_action")]
    public TemplateCardAction Action { get; set; }
    /// <summary>
    /// 创建一个Webhook模板卡片消息体
    /// </summary>
    /// <param name="action">整体卡片的点击跳转事件</param>
    /// <param name="mainTitle">模版卡片的主要内容</param>
    /// <param name="source">卡片来源样式信息</param>
    /// <param name="quoteArea">引用文献样式</param>
    /// <param name="horizontalContents">二级标题+文本列表,列表长度不超过6</param>
    /// <param name="jumps">跳转指引样式的列表,列表长度不超过3</param>
    /// <exception cref="ArgumentException"></exception>
    protected WebhookTemplateCardMessage(
        string cardType,
        TemplateCardAction action,
        TemplateCardMainTitle mainTitle = null,
        TemplateCardSource source = null,
        TemplateCardQuoteArea quoteArea = null,
        List<TemplateCardHorizontalContent> horizontalContents = null,
        List<TemplateCardJump> jumps = null)
    {
        CardType = Check.NotNullOrWhiteSpace(cardType, nameof(cardType));
        Action = Check.NotNull(action, nameof(action));

        MainTitle = mainTitle;
        Source = source;
        QuoteArea = quoteArea;
        HorizontalContents = horizontalContents;
        Jumps = jumps;

        if (horizontalContents != null && horizontalContents.Count > 6)
        {
            throw new ArgumentException("The length of the secondary title and the text list should not exceed 6!");
        }
        if (jumps != null && jumps.Count > 3)
        {
            throw new ArgumentException("The length of the list in the style of jump guidance should not exceed 3!");
        }
    }
}
