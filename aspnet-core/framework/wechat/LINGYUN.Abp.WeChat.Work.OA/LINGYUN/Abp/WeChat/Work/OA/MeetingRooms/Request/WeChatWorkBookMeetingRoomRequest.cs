using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
/// <summary>
/// 预定会议室请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93620#%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4"/>
/// </remarks>
public class WeChatWorkBookMeetingRoomRequest : WeChatWorkRequest
{
    /// <summary>
    /// 参与人的userid列表
    /// </summary>
    private readonly static List<string> _attendees = new List<string>();
    /// <summary>
    /// 会议室id
    /// </summary>
    [NotNull]
    [JsonProperty("meetingroom_id")]
    [JsonPropertyName("meetingroom_id")]
    public int MeetingRoomId { get; }
    /// <summary>
    /// 预定开始时间
    /// </summary>
    [NotNull]
    [JsonProperty("start_time")]
    [JsonPropertyName("start_time")]
    public long StartTime { get; }
    /// <summary>
    /// 预定结束时间
    /// </summary>
    [NotNull]
    [JsonProperty("end_time")]
    [JsonPropertyName("end_time")]
    public long EndTime { get; }
    /// <summary>
    /// 预定人的userid
    /// </summary>
    [NotNull]
    [JsonProperty("booker")]
    [JsonPropertyName("booker")]
    public string Booker { get; }
    /// <summary>
    /// 会议主题
    /// </summary>
    [CanBeNull]
    [JsonProperty("subject")]
    [JsonPropertyName("subject")]
    public string? Subject { get; }
    /// <summary>
    /// 参与人的userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("attendees")]
    [JsonPropertyName("attendees")]
    public string[] Attendees => _attendees.ToArray();
    public WeChatWorkBookMeetingRoomRequest(
        int meetingRoomId,
        DateTime startTime,
        DateTime endTime, 
        string booker,
        string? subject = null)
    {
        MeetingRoomId = Check.NotDefaultOrNull<int>(meetingRoomId, nameof(meetingRoomId));
        StartTime = startTime.GetUnixTimeSeconds();
        EndTime = endTime.GetUnixTimeSeconds();
        Booker = Check.NotNullOrWhiteSpace(booker, nameof(booker));
        Subject = subject;
    }
    /// <summary>
    /// 添加一个参与人
    /// </summary>
    /// <param name="userId"></param>
    public void AddAttendee(string userId)
    {
        _attendees.AddIfNotContains(userId);
    }
    /// <summary>
    /// 移除一个参与人
    /// </summary>
    /// <param name="userId"></param>
    public void RemoveAttendee(string userId)
    {
        _attendees.RemoveAll(attendee => string.Equals(attendee, userId));
    }
    /// <summary>
    /// 清空参与人
    /// </summary>
    public void ClearAttendee()
    {
        _attendees.Clear();
    }
}
