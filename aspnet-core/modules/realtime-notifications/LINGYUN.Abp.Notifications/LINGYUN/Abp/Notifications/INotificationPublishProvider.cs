using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知发布提供者接口
/// </summary>
public interface INotificationPublishProvider
{
    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 是否可发布通知
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    Task<bool> CanPublishAsync(
        NotificationInfo notification);
    /// <summary>
    /// 发布通知
    /// </summary>
    /// <param name="context">通知发送上下文信息</param>
    /// <returns></returns>
    Task PublishAsync(NotificationPublishContext context);
}
