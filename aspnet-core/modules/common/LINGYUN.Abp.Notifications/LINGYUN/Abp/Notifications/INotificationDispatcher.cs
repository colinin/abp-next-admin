using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    /// <summary>
    /// 通知发送者接口
    /// </summary>
    [Obsolete("Notification system redesigned, publisher interface deactivated, please use INotificationSender")]
    public interface INotificationDispatcher
    {
        ///// <summary>
        ///// 发送通知
        ///// </summary>
        ///// <param name="notificationName">通知名称</param>
        ///// <param name="data">数据</param>
        ///// <param name="tenantId">租户</param>
        ///// <param name="notificationSeverity">级别</param>
        ///// <returns></returns>
        //Task DispatchAsync(NotificationName notificationName, NotificationData data, Guid? tenantId = null, 
        //    NotificationSeverity notificationSeverity = NotificationSeverity.Info);

        ///// <summary>
        ///// 发送通知事件
        ///// </summary>
        ///// <param name="notificationName">通知名称</param>
        ///// <param name="data">数据</param>
        ///// <param name="tenantId">租户</param>
        ///// <param name="notificationSeverity">级别</param>
        ///// <returns></returns>
        //Task DispatchEventAsync(NotificationName notificationName, NotificationData data, Guid? tenantId = null,
        //    NotificationSeverity notificationSeverity = NotificationSeverity.Info);
    }
}
