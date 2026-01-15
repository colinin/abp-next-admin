using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
/// <summary>
/// 会议室详情
/// </summary>
public class MeetingRoomInfo
{
    /// <summary>
    /// 会议室id
    /// </summary>
    [NotNull]
    [JsonProperty("meetingroom_id")]
    [JsonPropertyName("meetingroom_id")]
    public int MeetingRoomId { get; set; }
    /// <summary>
    /// 会议室名称
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// 会议室容纳人数
    /// </summary>
    [NotNull]
    [JsonProperty("capacity")]
    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }
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
    /// 会议室支持的设备列表
    /// </summary>
    [NotNull]
    [JsonProperty("equipment")]
    [JsonPropertyName("equipment")]
    public int[] Equipment { get; set; }
    /// <summary>
    /// 会议室坐标
    /// </summary>
    [CanBeNull]
    [JsonProperty("coordinate")]
    [JsonPropertyName("coordinate")]
    public MeetingRoomCoordinate? Coordinate { get; set; }
    /// <summary>
    /// 会议室使用范围
    /// </summary>
    [CanBeNull]
    [JsonProperty("range")]
    [JsonPropertyName("range")]
    public MeetingRoomRange? Range { get; set; }
    /// <summary>
    /// 是否需要审批 0-无需审批 1-需要审批
    /// </summary>
    [NotNull]
    [JsonProperty("need_approval")]
    [JsonPropertyName("need_approval")]
    [Newtonsoft.Json.JsonConverter(typeof(IntToBoolConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(IntToBoolConverter))]
    public bool NeedApproval { get; set; }
}
