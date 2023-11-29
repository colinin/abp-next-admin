using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;
/// <summary>
/// 卡片右上角更多操作按钮
/// </summary>
public class TemplateCardActionMenu
{
    /// <summary>
    /// 卡片副交互辅助文本说明
    /// </summary>
    [CanBeNull]
    [JsonProperty("desc")]
    [JsonPropertyName("desc")]
    public string Description { get; set; }
    /// <summary>
    /// 操作列表，列表长度取值范围为 [1, 3]
    /// </summary>
    [NotNull]
    [JsonProperty("action_list")]
    [JsonPropertyName("action_list")]
    public List<TemplateCardAction> Actions { get; set;}
    public TemplateCardActionMenu(List<TemplateCardAction> actions, string description = "")
    {
        Actions = actions;
        Description = description;
    }
}
