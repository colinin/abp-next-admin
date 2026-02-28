using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 标签类型
/// </summary>
[Description("标签类型")]
public enum FollowUserTagType
{
    /// <summary>
    /// 企业设置
    /// </summary>
    [Description("企业设置")]
    EnterpriseSettings = 1,
    /// <summary>
    /// 用户自定义
    /// </summary>
    [Description("用户自定义")]
    UserCustom = 2,
    /// <summary>
    /// 规则组标签
    /// </summary>
    [Description("规则组标签")]
    RuleGroupTags = 3,
}
