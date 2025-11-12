using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Messages.Models;
/// <summary>
/// 成员的退群方式
/// </summary>
[Description("成员的退群方式")]
public enum ExternalChatMemberQuitScene
{
    /// <summary>
    /// 自己退群
    /// </summary>
    [Description("自己退群")]
    UserSelf = 0,
    /// <summary>
    /// 群主/群管理员移出
    /// </summary>
    [Description("群主/群管理员移出")]
    Admin = 1
}
