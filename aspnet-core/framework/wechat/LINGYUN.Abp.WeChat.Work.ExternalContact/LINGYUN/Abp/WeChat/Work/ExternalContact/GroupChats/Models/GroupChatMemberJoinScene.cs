using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Models;
/// <summary>
/// 入群方式
/// </summary>
[Description("入群方式")]
public enum GroupChatMemberJoinScene
{
    /// <summary>
    /// 由群成员邀请入群（直接邀请入群）
    /// </summary>
    [Description("直接邀请入群")]
    DirectInvitation = 1,
    /// <summary>
    /// 由群成员邀请入群（通过邀请链接入群）
    /// </summary>
    [Description("通过邀请链接入群")]
    InvitationLink = 2,
    /// <summary>
    /// 通过扫描群二维码入群
    /// </summary>
    [Description("通过扫描群二维码入群")]
    ScanQrCode = 3,
}
