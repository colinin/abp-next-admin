using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Response;
/// <summary>
/// 获取日历详情响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97717"/>
/// </remarks>
public class WeChatWorkGetCalendarListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 日历列表
    /// </summary>
    [NotNull]
    [JsonProperty("calendar_list")]
    [JsonPropertyName("calendar_list")]
    public CalendarInfo[] CalendarList { get; set; }
}
