using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 文本模板卡片消息
/// </summary>
public class TextTemplateCard : TemplateCard
{
    public TextTemplateCard(TemplateCardCardAction cardAction, string taskId = "")
        : base("template_card", taskId)
    {
        CardAction = cardAction;
    }

    /// <summary>
    /// 卡片来源样式信息，不需要来源样式可不填写
    /// </summary>
    [CanBeNull]
    [JsonProperty("source")]
    [JsonPropertyName("source")]
    public TemplateCardSource Source { get; set; }
    /// <summary>
    /// 卡片右上角更多操作按钮
    /// </summary>
    [CanBeNull]
    [JsonProperty("action_menu")]
    [JsonPropertyName("action_menu")]
    public TemplateCardActionMenu ActionMenu { get; set; }
    /// <summary>
    /// 一级标题
    /// </summary>
    [CanBeNull]
    [JsonProperty("main_title")]
    [JsonPropertyName("main_title")]
    public TemplateCardMainTitle MainTitle { get; set; }
    /// <summary>
    /// 二级普通文本，建议不超过160个字，（支持id转译）
    /// </summary>
    [CanBeNull]
    [JsonProperty("sub_title_text")]
    [JsonPropertyName("sub_title_text")]
    public string SubTitle { get; set; }
    /// <summary>
    /// 引用文献样式
    /// </summary>
    [CanBeNull]
    [JsonProperty("quote_area")]
    [JsonPropertyName("quote_area")]
    public TemplateCardQuoteArea QuoteArea { get; set; }
    /// <summary>
    /// 关键数据样式
    /// </summary>
    [CanBeNull]
    [JsonProperty("emphasis_content")]
    [JsonPropertyName("emphasis_content")]
    public TemplateCardEmphasisContent EmphasisContent { get; set; }
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
    /// 整体卡片的点击跳转事件，text_notice必填本字段
    /// </summary>
    [NotNull]
    [JsonProperty("TemplateCardCardAction")]
    [JsonPropertyName("TemplateCardCardAction")]
    public TemplateCardCardAction CardAction { get; set; }
}
