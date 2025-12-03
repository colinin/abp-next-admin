using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
/// <summary>
/// 预定状态
/// </summary>
[Description("预定状态")]
public enum MeetingRoomScheduleStatus
{
    /// <summary>
    /// 已预定
    /// </summary>
    [Description("已预定")]
    Reserved = 0,
    /// <summary>
    /// 申请中
    /// </summary>
    [Description("申请中")]
    Applying = 2,
    /// <summary>
    /// 审批中
    /// </summary>
    [Description("审批中")]
    Approvaling = 3,
}
