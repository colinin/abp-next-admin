using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 取消日程请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97725"/>
/// </remarks>
public class WeChatWorkDeleteScheduleRequest : WeChatWorkRequest
{
    /// <summary>
    /// 日程ID
    /// </summary>
    [NotNull]
    [JsonProperty("schedule_id")]
    [JsonPropertyName("schedule_id")]
    public string ScheduleId { get; }
    /// <summary>
    /// 操作模式。
    /// </summary>
    /// <remarks>
    /// 是重复日程时有效。
    /// </remarks>
    [CanBeNull]
    [JsonProperty("op_mode")]
    [JsonPropertyName("op_mode")]
    public ScheduleDeleteMode? OpMode { get; private set; }
    /// <summary>
    /// 操作起始时间。
    /// </summary>
    /// <remarks>
    /// 仅当操作模式是 DeleteThisSchedule 或 DeleteAllFutureSchedules 时有效。
    /// 该时间必须是重复日程的某一次开始时间。
    /// </remarks>
    [CanBeNull]
    [JsonProperty("op_start_time")]
    [JsonPropertyName("op_start_time")]
    public long? OpStartTime { get; private set; }
    public WeChatWorkDeleteScheduleRequest(string scheduleId)
    {
        ScheduleId = Check.NotNullOrWhiteSpace(scheduleId, nameof(scheduleId));
    }
    /// <summary>
    /// 删除此日程起始时间
    /// </summary>
    /// <param name="startTime"></param>
    public void DeleteThisSchedule(DateTime startTime)
    {
        OpMode = ScheduleDeleteMode.DeleteThisSchedule;
        OpStartTime = startTime.GetUnixTimeSeconds();
    }
    /// <summary>
    /// 删除所有日程起始时间
    /// </summary>
    /// <param name="startTime"></param>
    public void DeleteAllFutureSchedules(DateTime startTime)
    {
        OpMode = ScheduleDeleteMode.DeleteAllFutureSchedules;
        OpStartTime = startTime.GetUnixTimeSeconds();
    }
}
