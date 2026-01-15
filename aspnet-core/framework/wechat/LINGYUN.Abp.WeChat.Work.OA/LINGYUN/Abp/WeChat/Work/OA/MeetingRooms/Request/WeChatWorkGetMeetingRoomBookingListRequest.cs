using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
/// <summary>
/// 查询会议室的预定信息请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93620#%E6%9F%A5%E8%AF%A2%E4%BC%9A%E8%AE%AE%E5%AE%A4%E7%9A%84%E9%A2%84%E5%AE%9A%E4%BF%A1%E6%81%AF"/>
/// </remarks>
public class WeChatWorkGetMeetingRoomBookingListRequest : WeChatWorkRequest
{
    private readonly static DateTime _beginUnixTime = new DateTime(1970, 1, 1);
    /// <summary>
    /// 会议室id
    /// </summary>
    [CanBeNull]
    [JsonProperty("meetingroom_id")]
    [JsonPropertyName("meetingroom_id")]
    public int? MeetingRoomId { get; set; }
    /// <summary>
    /// 会议室所在城市
    /// </summary>
    [CanBeNull]
    [JsonProperty("city")]
    [JsonPropertyName("city")]
    public string? City { get; set; }
    /// <summary>
    /// 会议室所在楼宇
    /// </summary>
    [CanBeNull]
    [JsonProperty("building")]
    [JsonPropertyName("building")]
    public string? Building { get; set; }
    /// <summary>
    /// 会议室所在楼层
    /// </summary>
    [CanBeNull]
    [JsonProperty("floor")]
    [JsonPropertyName("floor")]
    public string? Floor { get; set; }
    /// <summary>
    /// 查询预定的起始时间，为空默认为当前时间
    /// </summary>
    [CanBeNull]
    [JsonProperty("start_time")]
    [JsonPropertyName("start_time")]
    public long? StartTime { get; private set; }
    /// <summary>
    /// 查询预定的结束时间， 为空默认为明日0时
    /// </summary>
    [CanBeNull]
    [JsonProperty("end_time")]
    [JsonPropertyName("end_time")]
    public long? EndTime { get; private set; }
    /// <summary>
    /// 设置查询预定时间范围
    /// </summary>
    /// <param name="startTime">起始时间</param>
    /// <param name="endTime">结束时间</param>
    public void Beetween(DateTime startTime, DateTime endTime)
    {
        StartTime = (long)(startTime - _beginUnixTime).TotalSeconds;
        EndTime = (long)(endTime - _beginUnixTime).TotalSeconds;
    }
}
