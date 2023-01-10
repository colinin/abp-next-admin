using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

public class EfCoreNotificationDefinitionRecordRepository :
    EfCoreRepository<INotificationsDbContext, NotificationDefinitionRecord, Guid>,
    INotificationDefinitionRecordRepository,
    ITransientDependency
{
    public EfCoreNotificationDefinitionRecordRepository(
        IDbContextProvider<INotificationsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
