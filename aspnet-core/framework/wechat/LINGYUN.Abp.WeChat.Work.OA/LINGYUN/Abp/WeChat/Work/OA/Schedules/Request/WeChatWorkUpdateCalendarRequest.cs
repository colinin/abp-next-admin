using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 更新日历请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97716"/>
/// </remarks>
public class WeChatWorkUpdateCalendarRequest : WeChatWorkRequest
{
    /// <summary>
    /// 日历信息
    /// </summary>
    [NotNull]
    [JsonProperty("calendar")]
    [JsonPropertyName("calendar")]
    public UpdateCalendar Calendar { get; }
    /// <summary>
    /// 是否不更新可订阅范围。0-否；1-是。默认会为0，会更新可订阅范围
    /// </summary>
    [CanBeNull]
    [JsonProperty("skip_public_range")]
    [JsonPropertyName("skip_public_range")]
    public uint? SkipPublicRange { get; }
    public WeChatWorkUpdateCalendarRequest(UpdateCalendar calendar, bool skipPublicRange = false)
    {
        Calendar = calendar;
        SkipPublicRange = skipPublicRange ? 1u : 0u;
    }

    protected override void Validate()
    {
        Check.NotNull(Calendar, nameof(Calendar));

        Calendar.Validate();
    }
}
