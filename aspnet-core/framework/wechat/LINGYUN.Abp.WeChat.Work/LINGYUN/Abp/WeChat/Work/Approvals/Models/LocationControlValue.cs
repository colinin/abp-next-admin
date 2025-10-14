using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 位置控件值
/// </summary>
public class LocationControlValue : ControlValue
{
    /// <summary>
    /// 位置
    /// </summary>
    [NotNull]
    [JsonProperty("location")]
    [JsonPropertyName("location")]
    public LocationValue Location { get; set; }
    public LocationControlValue()
    {

    }

    public LocationControlValue(LocationValue location)
    {
        Location = location;
    }
}

public class LocationValue
{
    /// <summary>
    /// 纬度，精确到6位小数
    /// </summary>
    [NotNull]
    [JsonProperty("latitude")]
    [JsonPropertyName("latitude")]
    public decimal Latitude { get; set; }
    /// <summary>
    /// 经度，精确到6位小数
    /// </summary>
    [NotNull]
    [JsonProperty("longitude")]
    [JsonPropertyName("longitude")]
    public decimal Longitude { get; set; }
    /// <summary>
    /// 地点标题
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 地点详情地址
    /// </summary>
    [NotNull]
    [JsonProperty("address")]
    [JsonPropertyName("address")]
    public string Address { get; set; }
    /// <summary>
    /// 选择地点的时间
    /// </summary>
    [NotNull]
    [JsonProperty("time")]
    [JsonPropertyName("time")]
    public long Time { get; set; }
    public LocationValue()
    {

    }

    public LocationValue(decimal latitude, decimal longitude, string title, string address, long time)
    {
        Latitude = latitude;
        Longitude = longitude;
        Title = title;
        Address = address;
        Time = time;
    }
}