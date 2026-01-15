using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 文本控件值
/// </summary>
public class TextControlValue : ControlValue
{
    /// <summary>
    /// 文本内容，在此填写文本/多行文本控件的输入值。文本控件Text内容不支持包含换行符。
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public string Text { get; set; }
    public TextControlValue()
    {

    }

    public TextControlValue(string text)
    {
        Text = text;
    }
}
