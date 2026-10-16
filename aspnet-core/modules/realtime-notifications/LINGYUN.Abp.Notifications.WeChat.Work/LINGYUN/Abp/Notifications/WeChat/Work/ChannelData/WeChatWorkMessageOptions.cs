using System;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信消息公共选项
/// </summary>
public class WeChatWorkMessageOptions
{
    /// <summary>
    /// 消息应用标识, 由渠道在发布时填充
    /// </summary>
    public string AgentId { get; set; } = default!;

    /// <summary>
    /// 是否是保密消息
    /// </summary>
    public bool? Safe { get; set; }

    /// <summary>
    /// 是否开启id转译
    /// </summary>
    public bool? EnableIdTrans { get; set; }

    /// <summary>
    /// 是否开启重复消息检查
    /// </summary>
    public bool? EnableDuplicateCheck { get; set; }

    /// <summary>
    /// 重复消息检查的时间间隔(秒)
    /// </summary>
    public int? DuplicateCheckInterval { get; set; }

    /// <summary>
    /// 从通知数据中解析公共选项
    /// </summary>
    public static WeChatWorkMessageOptions From(
        INotificationChannelDataResolver resolver,
        NotificationData notificationData)
    {
        Check.NotNull(resolver, nameof(resolver));
        Check.NotNull(notificationData, nameof(notificationData));

        return new WeChatWorkMessageOptions
        {
            Safe = resolver.GetValue<bool>(notificationData, WeChatWorkNotificationDataKeys.Safe),
            EnableIdTrans = resolver.GetValue<bool>(notificationData, WeChatWorkNotificationDataKeys.EnableIdTrans),
            EnableDuplicateCheck = resolver.GetValue<bool>(notificationData, WeChatWorkNotificationDataKeys.EnableDuplicateCheck),
            DuplicateCheckInterval = resolver.GetValue<int>(notificationData, WeChatWorkNotificationDataKeys.DuplicateCheckInterval)
        };
    }

    /// <summary>
    /// 保密消息标记
    /// </summary>
    public byte SafeValue => Safe == true ? (byte)1 : (byte)0;

    /// <summary>
    /// id转译标记
    /// </summary>
    public byte EnableIdTransValue => EnableIdTrans == true ? (byte)1 : (byte)0;

    /// <summary>
    /// 重复消息检查标记
    /// </summary>
    public byte EnableDuplicateCheckValue => EnableDuplicateCheck == true ? (byte)1 : (byte)0;

    /// <summary>
    /// 重复消息检查间隔, 未设置时使用企业微信默认值
    /// </summary>
    public int DuplicateCheckIntervalValue => DuplicateCheckInterval ?? 1800;
}
