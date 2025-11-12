using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Models;
/// <summary>
/// 成员类型
/// </summary>
[Description("成员类型")]
public enum GroupChatMemberType
{
    /// <summary>
    /// 企业成员
    /// </summary>
    [Description("企业成员")]
    Internal = 1,
    /// <summary>
    /// 外部联系人
    /// </summary>
    [Description("外部联系人")]
    External = 2
}
