using JetBrains.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 控件说明
/// </summary>
public class ControlPlaceholder
{
    /// <summary>
    /// 控件说明。需满足以下条件：长度不得超过80个字符。
    /// </summary>
    [NotNull]
    [StringLength(80)]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public string Text { get; set; }
    /// <summary>
    /// 显示语言，中文：zh_CN（注意不是zh-CN）；若text填写，则该项为必填
    /// </summary>
    [NotNull]
    [JsonProperty("lang")]
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    public ControlPlaceholder()
    {

    }

    public ControlPlaceholder(string text, string lang = "zh_CN")
    {
        Text = text;
        Lang = lang;
    }
}
