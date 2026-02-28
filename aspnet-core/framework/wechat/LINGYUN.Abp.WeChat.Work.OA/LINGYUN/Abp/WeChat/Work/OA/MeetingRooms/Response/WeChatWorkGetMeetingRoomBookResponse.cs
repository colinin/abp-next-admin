using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Response;
/// <summary>
/// 根据会议室预定ID查询预定详情响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93620#%E6%A0%B9%E6%8D%AE%E4%BC%9A%E8%AE%AE%E5%AE%A4%E9%A2%84%E5%AE%9Aid%E6%9F%A5%E8%AF%A2%E9%A2%84%E5%AE%9A%E8%AF%A6%E6%83%85"/>
/// </remarks>
public class WeChatWorkGetMeetingRoomBookResponse : WeChatWorkResponse
{
    /// <summary>
    /// 该会议室的预定情况
    /// </summary>
    [NotNull]
    [JsonProperty("schedule")]
    [JsonPropertyName("schedule")]
    public MeetingRoomSchedule Schedule { get; set; }
}
