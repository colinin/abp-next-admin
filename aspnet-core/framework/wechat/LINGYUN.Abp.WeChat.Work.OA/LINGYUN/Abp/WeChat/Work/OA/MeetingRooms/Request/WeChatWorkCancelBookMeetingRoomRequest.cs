using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
/// <summary>
/// 取消预定会议室请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93620#%E5%8F%96%E6%B6%88%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4"/>
/// </remarks>
public class WeChatWorkCancelBookMeetingRoomRequest : WeChatWorkRequest
{
    /// <summary>
    /// 会议室的预定id
    /// </summary>
    [NotNull]
    [JsonProperty("booking_id")]
    [JsonPropertyName("booking_id")]
    public string BookingId { get; }
    /// <summary>
    /// 是否保留日程，0-同步删除 1-保留，仅对非重复日程有效
    /// </summary>
    [NotNull]
    [JsonProperty("keep_schedule")]
    [JsonPropertyName("keep_schedule")]
    public int KeepSchedule { get; }
    /// <summary>
    /// 对于重复日程，如果不填写此参数，表示取消所有重复预定；如果填写，则表示取消对应日期当天的会议室预定
    /// </summary>
    [CanBeNull]
    [JsonProperty("cancel_date")]
    [JsonPropertyName("cancel_date")]
    public long? CancelDate { get; private set; }
    public WeChatWorkCancelBookMeetingRoomRequest(
        string bookingId,
        bool keepSchedule = false, 
        DateTime? cancelDate = null)
    {
        BookingId = Check.NotNullOrWhiteSpace(bookingId, nameof(bookingId));
        KeepSchedule = keepSchedule ? 1 : 0;
        if (cancelDate.HasValue)
        {
            CancelDate = (long)(cancelDate.Value - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
