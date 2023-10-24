using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

[ConnectionStringName(AbpNotificationsDbProperties.ConnectionStringName)]
public interface INotificationsDbContext : IEfCoreDbContext
{
    DbSet<Notification> Notifications { get; }
    DbSet<UserNotification> UserNotifications { get; }
    DbSet<UserSubscribe> UserSubscribes { get; }

    DbSet<NotificationDefinitionGroupRecord> NotificationDefinitionGroupRecords { get; }
    DbSet<NotificationDefinitionRecord> NotificationDefinitionRecords { get; }
}
