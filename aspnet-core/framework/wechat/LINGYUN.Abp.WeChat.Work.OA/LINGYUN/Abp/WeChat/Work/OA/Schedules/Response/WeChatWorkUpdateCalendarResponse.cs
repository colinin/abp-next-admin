using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Response;
/// <summary>
/// 更新日历响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97716"/>
/// </remarks>
public class WeChatWorkUpdateCalendarResponse : WeChatWorkResponse
{
    /// <summary>
    /// 无效的输入内容
    /// </summary>
    [NotNull]
    [JsonProperty("fail_result")]
    [JsonPropertyName("fail_result")]
    public CalendarFailResult FailResult { get; set; }
}
