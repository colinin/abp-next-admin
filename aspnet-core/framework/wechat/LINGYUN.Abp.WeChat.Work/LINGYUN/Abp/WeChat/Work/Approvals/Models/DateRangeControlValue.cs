using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 时长控件值
/// </summary>
public class DateRangeControlValue : ControlValue
{
    /// <summary>
    /// 时长
    /// </summary>
    [NotNull]
    [JsonProperty("date_range")]
    [JsonPropertyName("date_range")]
    public DateRangeValue DateRange { get; set; }
    public DateRangeControlValue()
    {

    }

    public DateRangeControlValue(DateRangeValue dateRange)
    {
        DateRange = dateRange;
    }
}

public class DateRangeValue
{
    /// <summary>
    /// 时长粒度：halfday:按天 hour:按小时
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }
    /// <summary>
    /// 开始时间,unix时间戳。当type 为halfday时，取值只能为固定两个时间点 上午：当天00:00:00点时间戳 下午：当天12:00:00时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("new_begin")]
    [JsonPropertyName("new_begin")]
    public long NewBegin { get; set; }
    /// <summary>
    /// 结束时间，unix时间戳。 当type 为halfday时，取值只能为固定两个时间点 上午：当天00:00:00点时间戳 下午：当天12:00:00时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("new_end")]
    [JsonPropertyName("new_end")]
    public long NewEnd { get; set; }
    /// <summary>
    /// 时长范围， 单位秒
    /// </summary>
    [NotNull]
    [JsonProperty("new_duration")]
    [JsonPropertyName("new_duration")]
    public long NewDuration { get; set; }
    public DateRangeValue()
    {

    }

    public DateRangeValue(string type, long newBegin, long newEnd, long newDuration)
    {
        Type = type;
        NewBegin = newBegin;
        NewEnd = newEnd;
        NewDuration = newDuration;
    }
}
