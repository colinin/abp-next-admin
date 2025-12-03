using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
/// <summary>
/// 通过会议预定会议室请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93620#%E9%80%9A%E8%BF%87%E4%BC%9A%E8%AE%AE%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4"/>
/// </remarks>
public class WeChatWorkBookMeetingRoomByMeetingRequest : WeChatWorkRequest
{
    /// <summary>
    /// 会议室id
    /// </summary>
    [NotNull]
    [JsonProperty("meetingroom_id")]
    [JsonPropertyName("meetingroom_id")]
    public int MeetingRoomId { get; }
    /// <summary>
    /// 预定人的userid
    /// </summary>
    [NotNull]
    [JsonProperty("booker")]
    [JsonPropertyName("booker")]
    public string Booker { get; }
    /// <summary>
    /// 会议id，仅可使用同应用创建的会议
    /// </summary>
    [NotNull]
    [JsonProperty("meetingid")]
    [JsonPropertyName("meetingid")]
    public string MeetingId { get; }
    public WeChatWorkBookMeetingRoomByMeetingRequest(
        int meetingRoomId,
        string booker,
        string meetingId)
    {
        MeetingRoomId = Check.NotDefaultOrNull<int>(meetingRoomId, nameof(meetingRoomId));
        Booker = Check.NotNullOrWhiteSpace(booker, nameof(booker));
        MeetingId = Check.NotNullOrWhiteSpace(meetingId, nameof(meetingId));
    }
}
