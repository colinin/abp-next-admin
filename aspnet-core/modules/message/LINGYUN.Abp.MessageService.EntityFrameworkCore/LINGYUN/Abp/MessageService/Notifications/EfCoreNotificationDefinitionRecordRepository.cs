using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Notifications;

public class EfCoreNotificationDefinitionRecordRepository :
    EfCoreRepository<IMessageServiceDbContext, NotificationDefinitionRecord, Guid>,
    INotificationDefinitionRecordRepository,
    ITransientDependency
{
    public EfCoreNotificationDefinitionRecordRepository(
        IDbContextProvider<IMessageServiceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
