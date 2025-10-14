using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 日期/日期+时间控件值
/// </summary>
public class DateControlValue : ControlValue
{
    /// <summary>
    /// 金额内容，在此填写金额控件的输入值
    /// </summary>
    [NotNull]
    [JsonProperty("date")]
    [JsonPropertyName("date")]
    public DateValue Date { get; set; }
    public DateControlValue()
    {

    }

    public DateControlValue(DateValue date)
    {
        Date = date;
    }
}

public class DateValue
{
    /// <summary>
    /// 时间展示类型：day-日期；hour-日期+时间 ，和对应模板控件属性一致
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }
    /// <summary>
    /// 时间戳-字符串类型，在此填写日期/日期+时间控件的选择值，以此为准
    /// </summary>
    [NotNull]
    [JsonProperty("s_timestamp")]
    [JsonPropertyName("s_timestamp")]
    public string Timestamp { get; set; }
    public DateValue()
    {

    }

    private DateValue(string type, string timestamp)
    {
        Type = type;
        Timestamp = timestamp;
    }

    public static DateValue Day(string timestamp)
    {
        return new DateValue("day", timestamp);
    }

    public static DateValue Hour(string timestamp)
    {
        return new DateValue("hour", timestamp);
    }
}
