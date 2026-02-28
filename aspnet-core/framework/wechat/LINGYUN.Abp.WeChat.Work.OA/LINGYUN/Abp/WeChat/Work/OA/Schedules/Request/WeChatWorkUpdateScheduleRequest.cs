using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 更新日程请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97720"/>
/// </remarks>
public class WeChatWorkUpdateScheduleRequest : WeChatWorkRequest
{
    /// <summary>
    /// 日程信息
    /// </summary>
    [NotNull]
    [JsonProperty("schedule")]
    [JsonPropertyName("schedule")]
    public UpdateSchedule Schedule { get; }
    /// <summary>
    /// 是否不更新参与人
    /// </summary>
    [CanBeNull]
    [JsonProperty("skip_attendees")]
    [JsonPropertyName("skip_attendees")]
    public uint? SkipAttendees { get; }
    /// <summary>
    /// 操作模式。
    /// </summary>
    /// <remarks>
    /// 是重复日程时有效。
    /// </remarks>
    [CanBeNull]
    [JsonProperty("op_mode")]
    [JsonPropertyName("op_mode")]
    public ScheduleUpdateMode? OpMode { get; private set; }
    /// <summary>
    /// 操作起始时间。
    /// </summary>
    /// <remarks>
    /// 仅当操作模式是 OnlyModifyThisSchedule 或 ModifyAllFutureSchedules 时有效。
    /// 该时间必须是重复日程的某一次开始时间。
    /// 例如：
    /// 假如日程开始时间start_time为1661990950（2022-09-01 08:09:10），且重复类型是每周，那么op_start_time可以是：1661990950（2022-09-01 08:09:10）、1662595750（2022-09-08 08:09:10）、1663200550（2022-09-15 08:09:10）......
    /// </remarks>
    [CanBeNull]
    [JsonProperty("op_start_time")]
    [JsonPropertyName("op_start_time")]
    public long? OpStartTime { get; private set; }
    public WeChatWorkUpdateScheduleRequest(UpdateSchedule schedule, bool skipAttendees = false)
    {
        Schedule = schedule;
        SkipAttendees = skipAttendees ? 1u : 0u;
    }
    /// <summary>
    /// 修改此日程起始时间
    /// </summary>
    /// <param name="startTime"></param>
    public void ModifyThisSchedule(DateTime startTime)
    {
        OpMode = ScheduleUpdateMode.ModifyThisSchedule;
        OpStartTime = startTime.GetUnixTimeSeconds();
    }
    /// <summary>
    /// 修改所有日程起始时间
    /// </summary>
    /// <param name="startTime"></param>
    public void ModifyAllFutureSchedules(DateTime startTime)
    {
        OpMode = ScheduleUpdateMode.ModifyAllFutureSchedules;
        OpStartTime = startTime.GetUnixTimeSeconds();
    }

    protected override void Validate()
    {
        Check.NotNull(Schedule, nameof(Schedule));

        Schedule.Validate();
    }
}
