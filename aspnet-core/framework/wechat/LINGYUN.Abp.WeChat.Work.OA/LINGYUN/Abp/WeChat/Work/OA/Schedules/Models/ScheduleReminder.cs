using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日程提醒
/// </summary>
public class ScheduleReminder
{
    /// <summary>
    /// 是否需要提醒
    /// </summary>
    [NotNull]
    [JsonProperty("is_remind")]
    [JsonPropertyName("is_remind")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool IsRemind { get; set; }
    /// <summary>
    /// 是否重复日程
    /// </summary>
    [NotNull]
    [JsonProperty("is_repeat")]
    [JsonPropertyName("is_repeat")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool IsRepeat { get; set; }
    /// <summary>
    /// 日程开始（start_time）前多少秒提醒，当is_remind为1时有效。例如： 300表示日程开始前5分钟提醒。目前仅支持以下数值：
    /// <list type="number">
    ///     <item>0 - 事件开始时</item>
    ///     <item>300 - 事件开始前5分钟</item>
    ///     <item>900 - 事件开始前15分钟</item>
    ///     <item>3600 - 事件开始前1小时</item>
    ///     <item>86400 - 事件开始前1天</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 注意：建议使用 remind_time_diffs 字段，该字段后续将会废弃。
    /// </remarks>
    [CanBeNull]
    [JsonProperty("remind_before_event_secs")]
    [JsonPropertyName("remind_before_event_secs")]
    public uint? RemindBeforeEventSecs { get; set; }
    /// <summary>
    /// 提醒时间与日程开始时间（start_time）的差值，当 IsRemind 为true时有效。例如：-300表示日程开始前5分钟提醒。
    /// </summary>
    /// <remarks>
    /// 特殊情况：企业微信终端设置的“全天”类型的日程，由于start_time是0点时间戳，提醒如果设置了当天9点，则会出现正数32400。
    /// 取值范围：-604800 ~ 86399
    /// </remarks>
    [CanBeNull]
    [JsonProperty("remind_time_diffs")]
    [JsonPropertyName("remind_time_diffs")]
    public int[]? RemindTimeDiffs { get; set; }
    /// <summary>
    /// 重复类型，当 IsRepeat 为true时有效
    /// </summary>
    [CanBeNull]
    [JsonProperty("repeat_type")]
    [JsonPropertyName("repeat_type")]
    public ScheduleReminderRepeatType? RepeatType { get; set; }
    /// <summary>
    /// 重复结束时刻，Unix时间戳。不填或填0表示一直重复
    /// </summary>
    [CanBeNull]
    [JsonProperty("repeat_until")]
    [JsonPropertyName("repeat_until")]
    public long? RepeatUntil { get; set; }
    /// <summary>
    /// 是否自定义重复
    /// </summary>
    [NotNull]
    [JsonProperty("is_custom_repeat")]
    [JsonPropertyName("is_custom_repeat")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool IsCustomRepeat { get; set; }
    /// <summary>
    /// 重复间隔
    /// </summary>
    /// <remarks>
    /// 仅当指定为自定义重复时有效。
    /// 该字段随repeat_type不同而含义不同。
    /// 例如：
    /// repeat_interval指定为2，repeat_type指定为每周重复，那么每2周重复一次；
    /// repeat_interval指定为2，repeat_type指定为每月重复，那么每2月重复一次
    /// </remarks>
    [CanBeNull]
    [JsonProperty("repeat_interval")]
    [JsonPropertyName("repeat_interval")]
    public uint? RepeatInterval { get; set; }
    /// <summary>
    /// 每周周几重复
    /// </summary>
    /// <remarks>
    /// 仅当指定为自定义重复且重复类型为每周时有效。
    /// 取值范围：1 ~ 7，分别表示周一至周日
    /// </remarks>
    [CanBeNull]
    [JsonProperty("repeat_day_of_week")]
    [JsonPropertyName("repeat_day_of_week")]
    public uint[]? RepeatDayOfWeek { get; set; }
    /// <summary>
    /// 每月哪几天重复
    /// </summary>
    /// <remarks>
    /// 仅当指定为自定义重复且重复类型为每月时有效。
    /// 取值范围：1 ~ 31，分别表示1~31号
    /// </remarks>
    [CanBeNull]
    [JsonProperty("repeat_day_of_month")]
    [JsonPropertyName("repeat_day_of_month")]
    public uint[]? RepeatDayOfMonth { get; set; }
    /// <summary>
    /// 时区。
    /// </summary>
    /// <remarks>
    /// UTC偏移量表示(即偏离零时区的小时数)，东区为正数，西区为负数。
    /// 例如：+8 表示北京时间东八区
    /// 默认为北京时间东八区
    /// 取值范围：-12 ~ +12
    /// </remarks>
    [CanBeNull]
    [JsonProperty("timezone")]
    [JsonPropertyName("timezone")]
    public uint? TimeZone { get; set; }
    /// <summary>
    /// 重复日程不包含的日期列表。
    /// </summary>
    /// <remarks>
    /// 对重复日程修改/删除特定一天或多天，则原来的日程将会排除对应的日期。
    /// </remarks>
    [CanBeNull]
    [JsonProperty("exclude_time_list")]
    [JsonPropertyName("exclude_time_list")]
    public ScheduleReminderExcludeTime[]? ExcludeTimeList { get; set; }
}
