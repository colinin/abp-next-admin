using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 获取日程详情请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97724"/>
/// </remarks>
public class WeChatWorkGetScheduleListRequest : WeChatWorkRequest
{
    private List<string> _scheduleIdList = new List<string>();
    /// <summary>
    /// 日程ID列表。一次最多拉取1000条
    /// </summary>
    [NotNull]
    [JsonProperty("schedule_id_list")]
    [JsonPropertyName("schedule_id_list")]
    public string[] ScheduleIdList => _scheduleIdList.ToArray();
    public WeChatWorkGetScheduleListRequest(string[] calIdList)
    {
        Check.NotNullOrEmpty(calIdList, nameof(calIdList));

        _scheduleIdList.AddIfNotContains(calIdList);
    }
    /// <summary>
    /// 添加日历Id
    /// </summary>
    /// <param name="calIdList"></param>
    public void WithCalId(params string[] calIdList)
    {
        _scheduleIdList.AddIfNotContains(calIdList);
    }

    protected override void Validate()
    {
        if (ScheduleIdList.Length > 2000)
        {
            throw new ArgumentException("A single schedule query can obtain up to 1,000 entries at most!", nameof(ScheduleIdList));
        }
    }
}
