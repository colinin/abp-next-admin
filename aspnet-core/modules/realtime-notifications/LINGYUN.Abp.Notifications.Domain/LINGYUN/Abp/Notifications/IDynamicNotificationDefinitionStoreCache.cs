using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications;

public interface IDynamicNotificationDefinitionStoreCache
{
    string CacheStamp { get; set; }

    SemaphoreSlim SyncSemaphore { get; }

    DateTime? LastCheckTime { get; set; }

    Task FillAsync(
        List<NotificationDefinitionGroupRecord> webhookGroupRecords,
        List<NotificationDefinitionRecord> webhookRecords);

    NotificationDefinition GetNotificationOrNull(string name);

    IReadOnlyList<NotificationDefinition> GetNotifications();

    NotificationGroupDefinition GetNotificationGroupOrNull(string name);

    IReadOnlyList<NotificationGroupDefinition> GetGroups();
}
