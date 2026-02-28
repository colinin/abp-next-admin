using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
/// <summary>
/// 添加会议室请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93619#%E6%B7%BB%E5%8A%A0%E4%BC%9A%E8%AE%AE%E5%AE%A4"/>
/// </remarks>
public class WeChatWorkCreateMeetingRoomRequest : WeChatWorkRequest
{
    /// <summary>
    /// 会议室名称
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; }
    /// <summary>
    /// 会议室容纳人数
    /// </summary>
    [NotNull]
    [JsonProperty("capacity")]
    [JsonPropertyName("capacity")]
    public int Capacity { get; }
    /// <summary>
    /// 会议室所在城市
    /// </summary>
    [CanBeNull]
    [JsonProperty("city")]
    [JsonPropertyName("city")]
    public string? City { get; private set; }
    /// <summary>
    /// 会议室所在楼宇
    /// </summary>
    [CanBeNull]
    [JsonProperty("building")]
    [JsonPropertyName("building")]
    public string? Building { get; private set; }
    /// <summary>
    /// 会议室所在楼层
    /// </summary>
    [CanBeNull]
    [JsonProperty("floor")]
    [JsonPropertyName("floor")]
    public string? Floor { get; private set; }
    /// <summary>
    /// 会议室支持的设备列表
    /// </summary>
    private readonly List<int> _equipment;
    /// <summary>
    /// 会议室支持的设备列表
    /// </summary>
    [NotNull]
    [JsonProperty("equipment")]
    [JsonPropertyName("equipment")]
    public int[] Equipment => _equipment.ToArray();
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
    public WeChatWorkCreateMeetingRoomRequest(string name, int capacity)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), 30);
        Capacity = Check.NotDefaultOrNull<int>(capacity, nameof(capacity));

        _equipment = new List<int>();
    }
    /// <summary>
    /// 设置位置信息
    /// </summary>
    /// <param name="city"></param>
    /// <param name="building"></param>
    /// <param name="floor"></param>
    public void WithLocation(string city, string building, string floor)
    {
        City = Check.NotNullOrWhiteSpace(city, nameof(city));
        Building = Check.NotNullOrWhiteSpace(building, nameof(building));
        Floor = Check.NotNullOrWhiteSpace(floor, nameof(floor));
    }
    /// <summary>
    /// 允许电视设备
    /// </summary>
    /// <returns></returns>
    public void WithTv()
    {
        _equipment.AddIfNotContains(1);
    }
    /// <summary>
    /// 允许电话设备
    /// </summary>
    /// <returns></returns>
    public void WithPhone()
    {
        _equipment.AddIfNotContains(2);

    }
    /// <summary>
    /// 允许投影设备
    /// </summary>
    /// <returns></returns>
    public void WithProjection()
    {
        _equipment.AddIfNotContains(3);
    }
    /// <summary>
    /// 允许白板设备
    /// </summary>
    /// <returns></returns>
    public void WithWhiteboard()
    {
        _equipment.AddIfNotContains(4);
    }
    /// <summary>
    /// 允许视频设备
    /// </summary>
    /// <returns></returns>
    public void WithVideo()
    {
        _equipment.AddIfNotContains(5);
    }
}
