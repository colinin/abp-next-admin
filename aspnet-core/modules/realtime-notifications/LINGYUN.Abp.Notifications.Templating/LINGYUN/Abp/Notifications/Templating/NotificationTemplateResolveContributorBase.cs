using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Templating;
public abstract class NotificationTemplateResolveContributorBase : INotificationTemplateResolveContributor
{
    public abstract string Name { get; }
    /// <summary>
    /// 实现此接口处理模板数据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public abstract Task ResolveAsync(INotificationTemplateResolveContext context);
}
