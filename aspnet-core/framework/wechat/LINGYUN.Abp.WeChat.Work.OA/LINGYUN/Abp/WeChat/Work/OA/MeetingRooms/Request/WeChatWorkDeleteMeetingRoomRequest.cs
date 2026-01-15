using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
/// <summary>
/// 删除会议室请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93619#%E5%88%A0%E9%99%A4%E4%BC%9A%E8%AE%AE%E5%AE%A4"/>
/// </remarks>
public class WeChatWorkDeleteMeetingRoomRequest : WeChatWorkRequest
{
    /// <summary>
    /// 会议室id
    /// </summary>
    [NotNull]
    [JsonProperty("meetingroom_id")]
    [JsonPropertyName("meetingroom_id")]
    public int MeetingRoomId { get; }
    public WeChatWorkDeleteMeetingRoomRequest(int meetingRoomId)
    {
        MeetingRoomId = Check.NotDefaultOrNull<int>(meetingRoomId, nameof(meetingRoomId));
    }
}
