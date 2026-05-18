using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications;
/// <summary>
/// 通知发布拦截器, 实现此接口来增加发布前的自定义检查
/// </summary>
public interface INotificationPublishInterceptor
{
    /// <summary>
    /// 拦截优先级，数值越小优先级越高
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// 在通知发布前执行检查
    /// </summary>
    /// <param name="notification">通知信息</param>
    /// <returns>true-允许发布, false-阻止发布</returns>
    Task<bool> CanPublishAsync(NotificationInfo notification);
}
