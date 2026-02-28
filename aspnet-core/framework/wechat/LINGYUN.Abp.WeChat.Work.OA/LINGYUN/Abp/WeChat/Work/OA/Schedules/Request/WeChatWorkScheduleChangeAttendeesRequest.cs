using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;

public abstract class WeChatWorkScheduleChangeAttendeesRequest : WeChatWorkRequest
{
    private List<ScheduleAttendee> _attendees = new List<ScheduleAttendee>();
    /// <summary>
    /// 日程ID。
    /// </summary>
    /// <remarks>
    /// 创建日程时返回的ID
    /// </remarks>
    [NotNull]
    [JsonProperty("schedule_id")]
    [JsonPropertyName("schedule_id")]
    public string ScheduleId { get; }
    /// <summary>
    /// 日程参与者列表
    /// </summary>
    /// <remarks>
    /// 累计最多支持1000人
    /// </remarks>
    [CanBeNull]
    [JsonProperty("attendees")]
    [JsonPropertyName("attendees")]
    public ScheduleAttendee[]? Attendees {
        get {
            if (_attendees.Count > 0)
            {
                return _attendees.ToArray();
            }
            return null;
        }
    }
    protected WeChatWorkScheduleChangeAttendeesRequest(string scheduleId)
    {
        ScheduleId = Check.NotNullOrWhiteSpace(scheduleId, nameof(scheduleId));
    }

    /// <summary>
    /// 添加一个日程参与者
    /// </summary>
    /// <param name="userId"></param>
    public void AddAttendee(string userId)
    {
        _attendees.Add(new ScheduleAttendee(userId));
    }
    /// <summary>
    /// 移除一个日程参与者
    /// </summary>
    /// <param name="userId"></param>
    public void RemoveAttendee(string userId)
    {
        _attendees.RemoveAll(x => x.UserId == userId);
    }
    /// <summary>
    /// 清空日程参与者
    /// </summary>
    public void ClearAttendee()
    {
        _attendees.Clear();
    }
}
