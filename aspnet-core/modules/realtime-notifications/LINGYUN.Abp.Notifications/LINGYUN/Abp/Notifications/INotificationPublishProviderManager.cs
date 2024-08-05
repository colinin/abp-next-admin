using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications;

public interface INotificationPublishProviderManager
{
    List<INotificationPublishProvider> Providers { get; }
}
