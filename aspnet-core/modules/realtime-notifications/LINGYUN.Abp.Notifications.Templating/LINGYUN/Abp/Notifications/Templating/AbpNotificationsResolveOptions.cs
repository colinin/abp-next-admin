using JetBrains.Annotations;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications.Templating;
public class AbpNotificationsResolveOptions
{
    /// <summary>
    /// 模板解析提供者列表
    /// </summary>
    [NotNull]
    public List<INotificationTemplateResolveContributor> TemplateResolvers { get; }

    public AbpNotificationsResolveOptions()
    {
        TemplateResolvers = new List<INotificationTemplateResolveContributor>();
    }
}
