using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 位置控件配置
/// </summary>
public class LocationControlConfig : ControlConfig
{
    /// <summary>
    /// 位置控件
    /// </summary>
    [NotNull]
    [JsonProperty("location")]
    [JsonPropertyName("location")]
    public LocationConfig Location { get; set; }
    public LocationControlConfig()
    {

    }

    public LocationControlConfig(LocationConfig location)
    {
        Location = location;
    }
}

public class LocationConfig
{
    /// <summary>
    /// 距离，目前支持100、200、300
    /// </summary>
    [NotNull]
    [JsonProperty("distance")]
    [JsonPropertyName("distance")]
    public int Distance { get; set; }
    public LocationConfig()
    {

    }

    public LocationConfig(int distance)
    {
        Distance = distance;
    }
}
