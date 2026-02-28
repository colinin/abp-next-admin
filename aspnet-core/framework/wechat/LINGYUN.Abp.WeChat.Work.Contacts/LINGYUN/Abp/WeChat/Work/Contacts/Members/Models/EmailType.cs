using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 邮箱类型
/// </summary>
[Description("邮箱类型")]
public enum EmailType
{
    /// <summary>
    /// 企业邮箱
    /// </summary>
    [Description("企业邮箱")]
    BizEmail = 1,
    /// <summary>
    /// 个人邮箱
    /// </summary>
    [Description("个人邮箱")]
    PersonEmail = 2,
}
