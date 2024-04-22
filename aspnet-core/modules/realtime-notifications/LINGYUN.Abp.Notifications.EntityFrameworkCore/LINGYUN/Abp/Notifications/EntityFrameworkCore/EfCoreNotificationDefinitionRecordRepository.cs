using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

public class EfCoreNotificationDefinitionRecordRepository :
    EfCoreRepository<INotificationsDefinitionDbContext, NotificationDefinitionRecord, Guid>,
    INotificationDefinitionRecordRepository,
    ITransientDependency
{
    public EfCoreNotificationDefinitionRecordRepository(
        IDbContextProvider<INotificationsDefinitionDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<NotificationDefinitionRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
