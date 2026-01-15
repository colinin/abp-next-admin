using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 日程状态
/// </summary>
[Description("日程状态")]
public enum ScheduleStatus : uint
{
    /// <summary>
    /// 正常
    /// </summary>
    [Description("正常")]
    Normal = 0,
    /// <summary>
    /// 已取消
    /// </summary>
    [Description("已取消")]
    Cancelled = 1,
}
