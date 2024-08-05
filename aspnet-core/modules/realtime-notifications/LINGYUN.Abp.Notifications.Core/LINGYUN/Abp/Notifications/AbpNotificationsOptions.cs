using System.Collections.Generic;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.Notifications;

public class AbpNotificationsOptions
{
    /// <summary>
    /// 自定义通知集合
    /// </summary>
    public ITypeList<INotificationDefinitionProvider> DefinitionProviders { get; }

    public HashSet<string> DeletedNotifications { get; }

    public HashSet<string> DeletedNotificationGroups { get; }

    public AbpNotificationsOptions()
    {
        DefinitionProviders = new TypeList<INotificationDefinitionProvider>();

        DeletedNotifications = new HashSet<string>();
        DeletedNotificationGroups = new HashSet<string>();
    }
}
