using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 日期/日期+时间控件配置
/// </summary>
public class DateControlConfig : ControlConfig
{
    /// <summary>
    /// 日期/日期+时间内容
    /// </summary>
    [NotNull]
    [JsonProperty("date")]
    [JsonPropertyName("date")]
    public DateConfig Date { get; set; }
    public DateControlConfig()
    {

    }

    public DateControlConfig(DateConfig date)
    {
        Date = date;
    }
}

public class DateConfig
{
    /// <summary>
    /// 时间展示类型：day-日期；hour-日期+时间 ，和对应模板控件属性一致
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }
}
