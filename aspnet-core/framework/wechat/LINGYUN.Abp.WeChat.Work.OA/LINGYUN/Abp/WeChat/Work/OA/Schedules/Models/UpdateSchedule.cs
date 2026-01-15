using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;

public class UpdateSchedule : CreateOrUpdateSchedule
{
    /// <summary>
    /// 日程ID
    /// </summary>
    /// <remarks>
    /// 创建日程时返回的ID
    /// </remarks>
    [NotNull]
    [JsonProperty("schedule_id")]
    [JsonPropertyName("schedule_id")]
    public string ScheduleId { get; }

    public UpdateSchedule(
        string scheduleId,
        DateTime startTime, 
        DateTime endTime) 
        : base(startTime, endTime)
    {
        ScheduleId = Check.NotNullOrWhiteSpace(scheduleId, nameof(scheduleId));
    }
}
