namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 新增日程参与者请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97721"/>
/// </remarks>
public class WeChatWorkScheduleAddAttendeesRequest : WeChatWorkScheduleChangeAttendeesRequest
{
    public WeChatWorkScheduleAddAttendeesRequest(string scheduleId) : base(scheduleId)
    {
    }
}
