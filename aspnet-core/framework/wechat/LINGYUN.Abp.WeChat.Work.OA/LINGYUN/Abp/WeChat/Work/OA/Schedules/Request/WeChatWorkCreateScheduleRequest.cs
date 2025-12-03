using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 创建日程请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/93648"/>
/// </remarks>
public class WeChatWorkCreateScheduleRequest : WeChatWorkRequest
{
    /// <summary>
    /// 日历信息
    /// </summary>
    [NotNull]
    [JsonProperty("schedule")]
    [JsonPropertyName("schedule")]
    public CreateSchedule Schedule { get; }
    /// <summary>
    /// 授权方安装的应用agentid。仅旧的第三方多应用套件需要填此参数
    /// </summary>
    [CanBeNull]
    [JsonProperty("agentid")]
    [JsonPropertyName("agentid")]
    public int? AgentId { get; set; }
    public WeChatWorkCreateScheduleRequest(CreateSchedule schedule)
    {
        Schedule = schedule;
    }

    protected override void Validate()
    {
        Check.NotNull(Schedule, nameof(Schedule));

        Schedule.Validate();
    }
}
