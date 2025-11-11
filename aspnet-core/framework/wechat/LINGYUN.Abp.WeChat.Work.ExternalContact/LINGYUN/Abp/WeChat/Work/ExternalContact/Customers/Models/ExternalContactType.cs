using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 外部联系人类型
/// </summary>
[Description("外部联系人类型")]
public enum ExternalContactType
{
    /// <summary>
    /// 微信用户
    /// </summary>
    [Description("微信用户")]
    WeChat = 1,
    /// <summary>
    /// 企业微信用户
    /// </summary>
    [Description("企业微信用户")]
    WeChatWork = 2,
}
