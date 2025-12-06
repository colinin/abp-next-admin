using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpNotificationsDbProperties.ConnectionStringName)]
public interface INotificationsDefinitionDbContext : IEfCoreDbContext
{
    DbSet<NotificationDefinitionGroupRecord> NotificationDefinitionGroupRecords { get; }
    DbSet<NotificationDefinitionRecord> NotificationDefinitionRecords { get; }
}
