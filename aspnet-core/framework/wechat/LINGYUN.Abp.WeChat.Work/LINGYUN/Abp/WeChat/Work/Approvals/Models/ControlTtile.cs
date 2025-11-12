using JetBrains.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 控件名称
/// </summary>
public class ControlTtile
{
    /// <summary>
    /// 控件名称。需满足以下条件：1-控件名称不得和现有控件名称重复；2-长度不得超过40个字符。3-Attendance-外出/出差/加班控件title固定为外出/出差/加班，暂不支持自定义
    /// </summary>
    [NotNull]
    [StringLength(40)]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public string Text { get; set; }
    /// <summary>
    /// 显示语言，中文：zh_CN（注意不是zh-CN）
    /// </summary>
    [NotNull]
    [JsonProperty("lang")]
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    public ControlTtile()
    {

    }

    public ControlTtile(string text, string lang = "zh_CN")
    {
        Text = text;
        Lang = lang;
    }
}
