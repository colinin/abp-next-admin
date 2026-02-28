using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 删除日历请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97718"/>
/// </remarks>
public class WeChatWorkDeleteCalendarRequest : WeChatWorkRequest
{
    /// <summary>
    /// 日历ID
    /// </summary>
    [NotNull]
    [JsonProperty("cal_id")]
    [JsonPropertyName("cal_id")]
    public string CalId { get; }
    public WeChatWorkDeleteCalendarRequest(string calId)
    {
        CalId = Check.NotNullOrWhiteSpace(calId, nameof(calId));
    }
}
