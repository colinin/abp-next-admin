using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Response;
/// <summary>
/// 获取日历下的日程列表响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97723"/>
/// </remarks>
public class WeChatWorkGetScheduleListByCalendarResponse : WeChatWorkResponse
{
    /// <summary>
    /// 日程列表
    /// </summary>
    [NotNull]
    [JsonProperty("schedule_list")]
    [JsonPropertyName("schedule_list")]
    public CalendarScheduleInfo[] ScheduleList { get; set; }
}
