using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    /// <summary>
    /// 发送通知接口
    /// </summary>
    public interface INotificationSender
    {
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="data">数据</param>
        /// <param name="userId">用户,为空标识发给所有订阅用户</param>
        /// <param name="tenantId">租户</param>
        /// <param name="severity">严重级别</param>
        /// <returns>通知标识</returns>
        Task<string> SendNofiterAsync(
            string name,
            NotificationData data,
            UserIdentifier user = null,
            Guid? tenantId = null,
            NotificationSeverity severity = NotificationSeverity.Info);
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="data">数据</param>
        /// <param name="users">用户列表,为空标识发给所有订阅用户</param>
        /// <param name="tenantId">租户</param>
        /// <param name="severity">严重级别</param>
        /// <returns>通知标识</returns>
        Task<string> SendNofitersAsync(
            string name,
            NotificationData data,
            IEnumerable<UserIdentifier> users = null,
            Guid? tenantId = null,
            NotificationSeverity severity = NotificationSeverity.Info);
    }
}
