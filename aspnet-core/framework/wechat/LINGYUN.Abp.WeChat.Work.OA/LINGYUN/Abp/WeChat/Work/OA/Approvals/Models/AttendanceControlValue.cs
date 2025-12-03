using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 假勤组件-出差/外出/加班组件值
/// </summary>
public class AttendanceControlValue : ControlValue
{
    /// <summary>
    /// 时长
    /// </summary>
    [NotNull]
    [JsonProperty("attendance")]
    [JsonPropertyName("attendance")]
    public AttendanceValue Attendance { get; set; }
    public AttendanceControlValue()
    {

    }

    public AttendanceControlValue(AttendanceValue attendance)
    {
        Attendance = attendance;
    }
}

public class AttendanceValue
{
    /// <summary>
    /// 假勤组件时间选择范围
    /// </summary>
    [NotNull]
    [JsonProperty("date_range")]
    [JsonPropertyName("date_range")]
    public DateRangeValue DateRange { get; set; }
    /// <summary>
    /// 假勤组件类型：1-请假；3-出差；4-外出；5-加班
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public byte Type { get; set; }
    /// <summary>
    /// 非必填。时长分片信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("slice_info")]
    [JsonPropertyName("slice_info")]
    public AttendanceSliceInfo SliceInfo { get; set; }
}

public class AttendanceSliceInfo
{
    /// <summary>
    /// 假勤组件类型：1-请假；3-出差；4-外出；5-加班
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public byte Type { get; set; }
    /// <summary>
    /// 某一天的分片
    /// </summary>
    [NotNull]
    [JsonProperty("day_items")]
    [JsonPropertyName("day_items")]
    public List<AttendanceSliceDayItem> DayItems { get; set; }
    public AttendanceSliceInfo()
    {

    }

    public AttendanceSliceInfo(byte type, List<AttendanceSliceDayItem> dayItems)
    {
        Type = type;
        DayItems = dayItems;
    }
}

public class AttendanceSliceDayItem
{
    /// <summary>
    /// 当天零点时间戳 （当天的东八区的零点时间戳）
    /// </summary>
    [NotNull]
    [JsonProperty("daytime")]
    [JsonPropertyName("daytime")]
    public long Daytime { get; set; }
    /// <summary>
    /// 某一天的时长，秒数，可以为0，（type为halfday时：加班：需为8640整倍数，其他：需为43200的整倍数，type为hour时需为360的整倍数）
    /// </summary>
    [NotNull]
    [JsonProperty("duration")]
    [JsonPropertyName("duration")]
    public long Duration { get; set; }
    public AttendanceSliceDayItem()
    {

    }

    public AttendanceSliceDayItem(long daytime, long duration)
    {
        Daytime = daytime;
        Duration = duration;
    }
}