using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications;
public static class INotificationSenderExtensions
{
    /// <summary>
    /// 发送通知
    /// </summary>
    /// <param name="sender">发送接口</param>
    /// <param name="name">名称</param>
    /// <param name="data">数据</param>
    /// <param name="user">用户,为空标识发给所有订阅用户</param>
    /// <param name="tenantId">租户</param>
    /// <param name="severity">严重级别</param>
    /// <returns>通知标识</returns>
    public async static Task<string> SendNofiterAsync(
        [NotNull] this INotificationSender sender,
        [NotNull] string name,
        [NotNull] NotificationData data,
        UserIdentifier user = null,
        Guid? tenantId = null,
        NotificationSeverity severity = NotificationSeverity.Info)
    {
        Check.NotNull(sender, nameof(sender));
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(data, nameof(data));

        if (user == null)
        {
            return await sender.SendNofiterAsync(name, data, tenantId: tenantId, severity: severity);
        }
        return await sender.SendNofiterAsync(name, data, new[] { user },tenantId: tenantId, severity: severity);
    }

    /// <summary>
    /// 发送模板通知
    /// </summary>
    /// <param name="sender">发送接口</param>
    /// <param name="name">名称</param>
    /// <param name="template">模板对象</param>
    /// <param name="user">用户,为空发给所有订阅用户</param>
    /// <param name="tenantId">租户</param>
    /// <param name="severity">严重级别</param>
    /// <returns>通知标识</returns>
    public async static Task<string> SendNofiterAsync(
        [NotNull] this INotificationSender sender,
        [NotNull] string name,
        [NotNull] NotificationTemplate template,
        UserIdentifier user = null,
        Guid? tenantId = null,
        NotificationSeverity severity = NotificationSeverity.Info)
    {
        Check.NotNull(sender, nameof(sender));
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(template, nameof(template));

        if (user == null)
        {
            return await sender.SendNofiterAsync(name, template, tenantId: tenantId, severity: severity);
        }
        return await sender.SendNofiterAsync(name, template, new[] { user }, tenantId: tenantId, severity: severity);
    }
}
