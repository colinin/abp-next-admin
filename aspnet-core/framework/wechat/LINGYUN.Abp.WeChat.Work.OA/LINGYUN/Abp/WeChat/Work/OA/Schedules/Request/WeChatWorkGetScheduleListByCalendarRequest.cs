using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 获取日历下的日程列表请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97723"/>
/// </remarks>
public class WeChatWorkGetScheduleListByCalendarRequest : WeChatWorkRequest
{
    /// <summary>
    /// 日历ID
    /// </summary>
    [NotNull]
    [JsonProperty("cal_id")]
    [JsonPropertyName("cal_id")]
    public string CalId { get; }
    /// <summary>
    /// 分页，偏移量, 默认为0
    /// </summary>
    [NotNull]
    [JsonProperty("offset")]
    [JsonPropertyName("offset")]
    public int Offset { get; }
    /// <summary>
    /// 分页，预期请求的数据量，默认为500，取值范围 1 ~ 1000
    /// </summary>
    [NotNull]
    [JsonProperty("limit")]
    [JsonPropertyName("limit")]
    public int Limit { get; }
    public WeChatWorkGetScheduleListByCalendarRequest(string calId, int offset = 0, int limit = 500)
    {
        CalId = Check.NotNullOrWhiteSpace(calId, nameof(calId));
        Offset = offset;
        Limit = limit;

        if (Limit <= 0)
        {
            Limit = 1;
        }
        if (Limit > 1000)
        {
            Limit = 1000;
        }
    }
}
