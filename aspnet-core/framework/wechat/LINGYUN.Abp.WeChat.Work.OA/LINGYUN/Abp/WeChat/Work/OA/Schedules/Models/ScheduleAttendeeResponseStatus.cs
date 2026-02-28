using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日程参与者的接受状态
/// </summary>
[Description("日程参与者的接受状态")]
public enum ScheduleAttendeeResponseStatus : uint
{
    /// <summary>
    /// 未处理
    /// </summary>
    [Description("未处理")]
    UnProcessed = 0,
    /// <summary>
    /// 待定
    /// </summary>
    [Description("待定")]
    Determined = 1,
    /// <summary>
    /// 全部接受
    /// </summary>
    [Description("全部接受")]
    AcceptAll = 2,
    /// <summary>
    /// 仅接受一次
    /// </summary>
    [Description("仅接受一次")]
    AcceptedOnce = 3,
    /// <summary>
    /// 拒绝
    /// </summary>
    [Description("拒绝")]
    Refuse = 4,
}
