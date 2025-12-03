using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 激活状态
/// </summary>
[Description("激活状态")]
public enum MemberStatus
{
    /// <summary>
    /// 已激活
    /// </summary>
    [Description("已激活")]
    Activated = 1,
    /// <summary>
    /// 已禁用
    /// </summary>
    [Description("已禁用")]
    Disabled = 2,
    /// <summary>
    /// 未激活
    /// </summary>
    [Description("未激活")]
    NotActivated = 4,
    /// <summary>
    /// 退出企业
    /// </summary>
    [Description("退出企业")]
    Exited = 5,
}
