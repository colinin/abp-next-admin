using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 数字控件值
/// </summary>
public class NumberControlValue : ControlValue
{
    /// <summary>
    /// 数字内容，在此填写数字控件的输入值
    /// </summary>
    [NotNull]
    [JsonProperty("new_number")]
    [JsonPropertyName("new_number")]
    public string NewMumber { get; set; }
    public NumberControlValue()
    {

    }

    public NumberControlValue(decimal newMumber)
    {
        NewMumber = newMumber.ToString();
    }
}
