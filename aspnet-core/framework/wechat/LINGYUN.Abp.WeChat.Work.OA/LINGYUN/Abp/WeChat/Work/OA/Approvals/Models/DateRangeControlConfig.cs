using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 时长组件配置
/// </summary>
public class DateRangeControlConfig : ControlConfig
{
    /// <summary>
    /// 时长组件
    /// </summary>
    [NotNull]
    [JsonProperty("date_range")]
    [JsonPropertyName("date_range")]
    public DateRangeConfig DateRange { get; set; }
    public DateRangeControlConfig()
    {

    }

    public DateRangeControlConfig(DateRangeConfig dateRange)
    {
        DateRange = dateRange;
    }
}
