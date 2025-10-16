using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;
/// <summary>
/// Webhook 图文展示模版卡片消息体
/// </summary>
public class WebhookNewsNoticeCardMessage : WebhookTemplateCardMessage
{
    /// <summary>
    /// 图片样式
    /// </summary>
    [NotNull]
    [JsonProperty("aspect_ratio")]
    [JsonPropertyName("aspect_ratio")]
    public WebhookTemplateCardImage CardImage { get; set; }
    /// <summary>
    /// 左图右文样式
    /// </summary>
    [CanBeNull]
    [JsonProperty("image_text_area")]
    [JsonPropertyName("image_text_area")]
    public WebhookTemplateCardImageTextArea ImageTextArea { get; set; }
    /// <summary>
    /// 卡片二级垂直内容，该字段可为空数组，但有数据的话需确认对应字段是否必填，列表长度不超过4
    /// </summary>
    [CanBeNull]
    [JsonProperty("vertical_content_list")]
    [JsonPropertyName("vertical_content_list")]
    public List<WebhookTemplateCardVerticalContent> VerticalContents { get; set; }
    /// <summary>
    /// 创建一个Webhook 图文展示模版卡片消息体
    /// </summary>
    /// <param name="cardImage">图片样式</param>
    /// <param name="action">整体卡片的点击跳转事件</param>
    /// <param name="mainTitle">模版卡片的主要内容</param>
    /// <param name="imageTextArea">左图右文样式</param>
    /// <param name="source">卡片来源样式信息</param>
    /// <param name="quoteArea">引用文献样式</param>
    /// <param name="horizontalContents">二级标题+文本列表,列表长度不超过6</param>
    /// <param name="verticalContents">卡片二级垂直内容,列表长度不超过4</param>
    /// <param name="jumps">跳转指引样式的列表,列表长度不超过3</param>
    /// <exception cref="ArgumentException"></exception>
    public WebhookNewsNoticeCardMessage(
        WebhookTemplateCardImage cardImage,
        WebhookTemplateCardAction action,
        WebhookTemplateCardMainTitle mainTitle,
        WebhookTemplateCardImageTextArea imageTextArea = null,
        WebhookTemplateCardSource source = null,
        WebhookTemplateCardQuoteArea quoteArea = null, 
        List<WebhookTemplateCardHorizontalContent> horizontalContents = null,
        List<WebhookTemplateCardVerticalContent> verticalContents = null,
        List<WebhookTemplateCardJump> jumps = null) 
        : base("news_notice", action, mainTitle, source, quoteArea, horizontalContents, jumps)
    {
        Check.NotNull(mainTitle, nameof(mainTitle));
        Check.NotNull(cardImage, nameof(cardImage));

        CardImage = cardImage;
        ImageTextArea = imageTextArea;
        VerticalContents = verticalContents;

        if (verticalContents != null && verticalContents.Count > 4)
        {
            throw new ArgumentException("The length of the secondary vertical content list on the card cannot exceed 4!");
        }
    }
}
