using System;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信消息接收者
/// </summary>
/// <remarks>
/// 企业微信要求 touser / toparty / totag 不能同时为空,
/// 因此接收者必须作为整体传递给消息构造过程。
/// </remarks>
public class WeChatWorkNotificationContact
{
    /// <summary>
    /// 成员ID列表, 多个接收者使用"|"分隔
    /// </summary>
    public string? ToUser { get; set; }

    /// <summary>
    /// 部门ID列表, 多个接收者使用"|"分隔
    /// </summary>
    public string? ToParty { get; set; }

    /// <summary>
    /// 标签ID列表, 多个接收者使用"|"分隔
    /// </summary>
    public string? ToTag { get; set; }

    public bool IsEmpty()
    {
        return ToUser.IsNullOrWhiteSpace()
            && ToParty.IsNullOrWhiteSpace()
            && ToTag.IsNullOrWhiteSpace();
    }

    public static WeChatWorkNotificationContact From(
        string? toUser = null,
        string? toParty = null,
        string? toTag = null)
    {
        return new WeChatWorkNotificationContact
        {
            ToUser = toUser,
            ToParty = toParty,
            ToTag = toTag
        };
    }
}
