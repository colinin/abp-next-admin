using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

public class EfCoreNotificationDefinitionGroupRecordRepository :
    EfCoreRepository<INotificationsDefinitionDbContext, NotificationDefinitionGroupRecord, Guid>,
    INotificationDefinitionGroupRecordRepository,
    ITransientDependency
{
    public EfCoreNotificationDefinitionGroupRecordRepository(
        IDbContextProvider<INotificationsDefinitionDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
