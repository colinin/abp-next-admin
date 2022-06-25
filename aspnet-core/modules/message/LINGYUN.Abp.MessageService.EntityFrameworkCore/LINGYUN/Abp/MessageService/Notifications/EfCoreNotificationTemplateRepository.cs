using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Notifications;

public class EfCoreNotificationTemplateRepository :
    EfCoreRepository<IMessageServiceDbContext, NotificationTemplate, Guid>,
    INotificationTemplateRepository,
    ITransientDependency
{
    public EfCoreNotificationTemplateRepository(
        IDbContextProvider<IMessageServiceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<NotificationTemplate> GetByNameAsync(string name, string culture = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Name.Equals(name))
            .WhereIf(!culture.IsNullOrWhiteSpace(), x => x.Culture.Equals(culture))
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
