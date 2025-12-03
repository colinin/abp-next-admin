using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
/// <summary>
/// 会议室坐标
/// </summary>
public class MeetingRoomCoordinate
{
    /// <summary>
    /// 会议室所在楼宇的纬度
    /// </summary>
    [NotNull]
    [JsonProperty("latitude")]
    [JsonPropertyName("latitude")]
    public string Latitude { get; }
    /// <summary>
    /// 会议室所在楼宇的经度
    /// </summary>
    [NotNull]
    [JsonProperty("longitude")]
    [JsonPropertyName("longitude")]
    public string Longitude { get; }
    public MeetingRoomCoordinate(decimal latitude, decimal longitude)
    {
        Latitude = latitude.ToString(); 
        Longitude = longitude.ToString();
    }
}
