using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
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
        /// 发布通知
        /// </summary>
        /// <param name="notification">通知信息</param>
        /// <param name="identifiers">接收用户列表</param>
        /// <returns></returns>
        Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers);
    }
}
