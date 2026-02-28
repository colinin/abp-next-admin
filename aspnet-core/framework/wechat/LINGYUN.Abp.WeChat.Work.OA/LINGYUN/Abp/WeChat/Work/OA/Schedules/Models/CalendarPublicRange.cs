using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日历公开范围
/// </summary>
public class CalendarPublicRange
{
    /// <summary>
    /// 公开的成员列表范围
    /// </summary>
    [CanBeNull]
    [JsonProperty("userids")]
    [JsonPropertyName("userids")]
    public string[]? UserIds { get; set; }
    /// <summary>
    /// 公开的部门列表范围
    /// </summary>
    [CanBeNull]
    [JsonProperty("partyids")]
    [JsonPropertyName("partyids")]
    public int[]? PartyIds { get; set; }
    public CalendarPublicRange(string[]? userIds = null, int[]? partyIds = null)
    {
        UserIds = userIds;
        PartyIds = partyIds;
    }
}
