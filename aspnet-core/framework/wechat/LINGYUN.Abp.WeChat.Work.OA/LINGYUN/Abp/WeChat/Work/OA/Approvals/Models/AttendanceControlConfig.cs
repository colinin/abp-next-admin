using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 假勤组件-出差/外出/加班组件配置
/// </summary>
public class AttendanceControlConfig : ControlConfig
{
    /// <summary>
    /// 假勤组件-出差/外出/加班组件
    /// </summary>
    [NotNull]
    [JsonProperty("attendance")]
    [JsonPropertyName("attendance")]
    public AttendanceConfig Attendance { get; set; }
    public AttendanceControlConfig()
    {

    }

    public AttendanceControlConfig(AttendanceConfig attendance)
    {
        Attendance = attendance;
    }
}

public class AttendanceConfig
{
    /// <summary>
    /// 假勤组件类型：3-出差；4-外出；5-加班
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public byte Type { get; set; }
    /// <summary>
    /// 假勤组件时间选择范围
    /// </summary>
    [NotNull]
    [JsonProperty("date_range")]
    [JsonPropertyName("date_range")]
    public DateRangeConfig DateRange { get; set; }
    public AttendanceConfig()
    {

    }

    public AttendanceConfig(byte type, DateRangeConfig dateRange)
    {
        Type = type;
        DateRange = dateRange;
    }
}