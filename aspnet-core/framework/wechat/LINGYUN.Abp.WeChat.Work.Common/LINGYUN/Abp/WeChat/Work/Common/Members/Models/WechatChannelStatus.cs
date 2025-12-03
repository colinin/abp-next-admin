using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Common.Members.Models;
/// <summary>
/// 视频号状态
/// </summary>
[Description("视频号状态")]
public enum WechatChannelStatus
{
    /// <summary>
    /// 已确认
    /// </summary>
    [Description("已确认")]
    Confirmed = 0,
    /// <summary>
    /// 待确认
    /// </summary>
    [Description("待确认")]
    UnConfirmed = 1,
}
