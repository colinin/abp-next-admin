using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 时长配置
/// </summary>
public class DateRangeConfig
{
    /// <summary>
    /// 时间展示类型：halfday-日期；hour-日期+时间
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }
    /// <summary>
    /// 0-自然日；1-工作日
    /// </summary>
    [NotNull]
    [JsonProperty("official_holiday")]
    [JsonPropertyName("official_holiday")]
    public byte OfficialHoliday { get; set; }
    /// <summary>
    /// 一天的时长（单位为秒），必须大于0小于等于86400
    /// </summary>
    [NotNull]
    [JsonProperty("perday_duration")]
    [JsonPropertyName("perday_duration")]
    public int PerdayDuration { get; set; }
    public DateRangeConfig()
    {

    }

    public DateRangeConfig(string type, byte officialHoliday, int perdayDuration)
    {
        Type = type;
        OfficialHoliday = officialHoliday;
        PerdayDuration = perdayDuration;
    }
}
