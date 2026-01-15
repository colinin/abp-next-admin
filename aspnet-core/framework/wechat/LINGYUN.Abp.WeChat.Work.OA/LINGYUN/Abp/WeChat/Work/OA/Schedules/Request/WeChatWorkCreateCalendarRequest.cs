using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 创建日历请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93647"/>
/// </remarks>
public class WeChatWorkCreateCalendarRequest : WeChatWorkRequest
{
    /// <summary>
    /// 日历信息
    /// </summary>
    [NotNull]
    [JsonProperty("calendar")]
    [JsonPropertyName("calendar")]
    public CreateCalendar Calendar { get; }
    /// <summary>
    /// 授权方安装的应用agentid。仅旧的第三方多应用套件需要填此参数
    /// </summary>
    [CanBeNull]
    [JsonProperty("agentid")]
    [JsonPropertyName("agentid")]
    public int? AgentId { get; set; }
    public WeChatWorkCreateCalendarRequest(CreateCalendar calendar)
    {
        Calendar = calendar;
    }

    protected override void Validate()
    {
        Check.NotNull(Calendar, nameof(Calendar));

        Calendar.Validate();
    }
}
