using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 模版名称定义
/// </summary>
public class TemplateName
{
    /// <summary>
    /// 模板名称
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public string Text {  get; set; }
    /// <summary>
    /// 多语言名称
    /// </summary>
    [NotNull]
    [JsonProperty("lang")]
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    public TemplateName()
    {

    }

    public TemplateName(string text, string lang = "zh_CN")
    {
        Text = text;
        Lang = lang;
    }
}
