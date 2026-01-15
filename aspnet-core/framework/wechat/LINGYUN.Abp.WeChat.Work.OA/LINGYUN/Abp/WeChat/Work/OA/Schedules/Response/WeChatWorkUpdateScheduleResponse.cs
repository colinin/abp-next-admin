using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Response;
/// <summary>
/// 更新日程响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97720"/>
/// </remarks>
public class WeChatWorkUpdateScheduleResponse : WeChatWorkResponse
{
    /// <summary>
    /// 修改重复日程新产生的日程ID。
    /// </summary>
    /// <remarks>
    /// 对于重复日程，如果不是修改全部周期，会修剪原重复日程，产生新的重复日程，此时会返回新日程的ID
    /// </remarks>
    [NotNull]
    [JsonProperty("schedule_id")]
    [JsonPropertyName("schedule_id")]
    public string ScheduleId { get; set; }
}
