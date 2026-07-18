using System.ComponentModel;

namespace LINGYUN.Abp.Dingtalk.Messages.Notifications.Models;
/// <summary>
/// DING消息类型
/// </summary>
[Description("DING消息类型")]
public enum RemindType
{
    /// <summary>
    /// 应用内DING
    /// </summary>
    [Description("应用内DING")]
    Application = 1,
    /// <summary>
    /// 短信DING
    /// </summary>
    [Description("短信DING")]
    Sms = 2,
    /// <summary>
    /// 电话DING
    /// </summary>
    [Description("电话DING")]
    Tel = 3,
}
