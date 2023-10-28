using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
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

    public async virtual Task<NotificationDefinitionGroupRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
