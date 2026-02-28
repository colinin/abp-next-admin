namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
/// <summary>
/// 删除日程参与者
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97722"/>
/// </remarks>
public class WeChatWorkScheduleDeleteAttendeesRequest : WeChatWorkScheduleChangeAttendeesRequest
{
    public WeChatWorkScheduleDeleteAttendeesRequest(string scheduleId) : base(scheduleId)
    {
    }
}
