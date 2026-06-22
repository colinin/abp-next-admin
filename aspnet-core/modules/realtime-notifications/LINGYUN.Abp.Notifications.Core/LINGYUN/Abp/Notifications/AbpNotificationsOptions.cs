using System.Collections.Generic;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.Notifications;

public class AbpNotificationsOptions
{
    /// <summary>
    /// 自定义通知集合
    /// </summary>
    public ITypeList<INotificationDefinitionProvider> DefinitionProviders { get; }

    public DynamicNotificationStrategy DynamicNotificationStrategy { get; set; }

    public HashSet<string> DeletedNotifications { get; }

    public HashSet<string> DeletedNotificationGroups { get; }

    public AbpNotificationsOptions()
    {
        DynamicNotificationStrategy = DynamicNotificationStrategy.Merge;
        DefinitionProviders = new TypeList<INotificationDefinitionProvider>();

        DeletedNotifications = new HashSet<string>();
        DeletedNotificationGroups = new HashSet<string>();
    }
}
