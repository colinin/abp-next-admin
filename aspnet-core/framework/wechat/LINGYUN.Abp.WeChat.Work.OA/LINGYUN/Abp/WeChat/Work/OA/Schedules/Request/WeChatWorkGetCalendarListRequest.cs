using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 获取日历详情请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97717"/>
/// </remarks>
public class WeChatWorkGetCalendarListRequest : WeChatWorkRequest
{
    private List<string> _calIdList = new List<string>();
    /// <summary>
    /// 日历ID列表，调用创建日历接口后获得。一次最多可获取1000条
    /// </summary>
    [NotNull]
    [JsonProperty("cal_id_list")]
    [JsonPropertyName("cal_id_list")]
    public string[] CalIdList => _calIdList.ToArray();
    public WeChatWorkGetCalendarListRequest(string[] calIdList)
    {
        Check.NotNullOrEmpty(calIdList, nameof(calIdList));

        _calIdList.AddIfNotContains(calIdList);
    }
    /// <summary>
    /// 添加日历Id
    /// </summary>
    /// <param name="calIdList"></param>
    public void WithCalId(params string[] calIdList)
    {
        _calIdList.AddIfNotContains(calIdList);
    }

    protected override void Validate()
    {
        if (CalIdList.Length > 2000)
        {
            throw new ArgumentException("A single calendar query can obtain up to 1,000 entries at most!", nameof(CalIdList));
        }
    }
}
