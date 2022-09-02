using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Notifications;

public class EfCoreNotificationDefinitionGroupRecordRepository :
    EfCoreRepository<IMessageServiceDbContext, NotificationDefinitionGroupRecord, Guid>,
    INotificationDefinitionGroupRecordRepository,
    ITransientDependency
{
    public EfCoreNotificationDefinitionGroupRecordRepository(
        IDbContextProvider<IMessageServiceDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
