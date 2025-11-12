using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 成员的入群方式
/// </summary>
[Description("成员的入群方式")]
public enum ExternalChatMemberJoinScene
{
    /// <summary>
    /// 由成员邀请入群
    /// </summary>
    [Description("由成员邀请入群")]
    MemberInvitation = 0,
    /// <summary>
    /// 通过扫描群二维码入群
    /// </summary>
    [Description("通过扫描群二维码入群")]
    ScanQrCode = 3
}
