using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 更新日历
/// </summary>
public class UpdateCalendar : CreateOrUpdateCalendar
{
    /// <summary>
    /// 日历ID
    /// </summary>
    [NotNull]
    [JsonProperty("cal_id")]
    [JsonPropertyName("cal_id")]
    public string CalId { get; }

    public UpdateCalendar(
        string calId,
        string summary, 
        string color,
        string? description)
        : base(summary, color, description)
    {
        CalId = Check.NotNullOrWhiteSpace(calId, nameof(calId));
    }
}
