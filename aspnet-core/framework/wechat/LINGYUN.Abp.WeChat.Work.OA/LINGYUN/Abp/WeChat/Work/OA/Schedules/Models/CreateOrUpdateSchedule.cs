using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;

public abstract class CreateOrUpdateSchedule
{
    /// <summary>
    /// 日程的管理员userid列表，管理员必须在共享成员的列表中
    /// </summary>
    /// <remarks>
    /// 最多指定3人
    /// </remarks>
    [CanBeNull]
    [JsonProperty("admins")]
    [JsonPropertyName("admins")]
    public string[]? Admins { get; set; }
    /// <summary>
    /// 日程参与者列表
    /// </summary>
    /// <remarks>
    /// 最多支持1000人
    /// </remarks>
    [CanBeNull]
    [JsonProperty("attendees")]
    [JsonPropertyName("attendees")]
    public ScheduleAttendee[]? Attendees { get; set; }
    /// <summary>
    /// 日程标题
    /// </summary>
    /// <remarks>
    /// 0 ~ 128 字符。不填会默认显示为“新建事件”
    /// </remarks>
    [CanBeNull]
    [JsonProperty("summary")]
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }
    /// <summary>
    /// 日程描述
    /// </summary>
    /// <remarks>
    /// 不多于1000个字符
    /// </remarks>
    [CanBeNull]
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    /// <summary>
    /// 提醒相关信息
    /// </summary>
    [CanBeNull]
    [JsonProperty("reminders")]
    [JsonPropertyName("reminders")]
    public ScheduleReminder? Reminders { get; set; }
    /// <summary>
    /// 日程地址
    /// </summary>
    /// <remarks>
    /// 不多于128个字符
    /// </remarks>
    [CanBeNull]
    [JsonProperty("location")]
    [JsonPropertyName("location")]
    public string? Location { get; set; }
    /// <summary>
    /// 日程开始时间，Unix时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("start_time")]
    [JsonPropertyName("start_time")]
    public long StartTime { get; }
    /// <summary>
    /// 日程结束时间，Unix时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("end_time")]
    [JsonPropertyName("end_time")]
    public long EndTime { get; }
    /// <summary>
    /// 日程所属日历ID
    /// </summary>
    /// <remarks>
    /// 不多于64字节
    /// </remarks>
    [CanBeNull]
    [JsonProperty("cal_id")]
    [JsonPropertyName("cal_id")]
    public string? CalId { get; set; }
    /// <summary>
    /// 是否全天日程
    /// </summary>
    [NotNull]
    [JsonProperty("is_whole_day")]
    [JsonPropertyName("is_whole_day")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool? IsWholeDay { get; set; }
    protected CreateOrUpdateSchedule(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime.GetUnixTimeSeconds();
        EndTime = endTime.GetUnixTimeSeconds();
    }

    /// <summary>
    /// 验证输入
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Validate()
    {
        Check.Length(Summary, nameof(Summary), 128);
        Check.Length(Description, nameof(Description), 1000);
        Check.Length(Location, nameof(Location), 128);
        Check.Length(CalId, nameof(CalId), 64);

        if (Admins?.Length > 3)
        {
            throw new ArgumentException("Admins list. Up to 3!", nameof(Admins));
        }

        if (Attendees?.Length > 1000)
        {
            throw new ArgumentException("Attendees list. Up to 1000!", nameof(Attendees));
        }
    }
}
