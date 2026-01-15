using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Response;
/// <summary>
/// 查询会议室响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93619#%E6%9F%A5%E8%AF%A2%E4%BC%9A%E8%AE%AE%E5%AE%A4"/>
/// </remarks>
public class WeChatWorkGetMeetingRoomListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 会议室列表
    /// </summary>
    [NotNull]
    [JsonProperty("meetingroom_list")]
    [JsonPropertyName("meetingroom_list")]
    public MeetingRoomInfo[] MeetingRoomList { get; set; }
}
