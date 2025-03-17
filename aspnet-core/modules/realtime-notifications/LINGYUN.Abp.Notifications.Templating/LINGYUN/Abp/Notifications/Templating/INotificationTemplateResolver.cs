using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Templating;
/// <summary>
/// 通知模板模型解析接口
/// </summary>
public interface INotificationTemplateResolver
{
    /// <summary>
    /// 解析模板数据
    /// </summary>
    /// <param name="template"></param>
    /// <returns></returns>
    [NotNull]
    Task<NotificationTemplateResolveResult> ResolveAsync(NotificationTemplate template);
}
