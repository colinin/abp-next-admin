using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
public class EfCoreBackgroundJobActionRepository :
    EfCoreRepository<TaskManagementDbContext, BackgroundJobAction, Guid>,
    IBackgroundJobActionRepository
{
    public EfCoreBackgroundJobActionRepository(
        IDbContextProvider<TaskManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<List<BackgroundJobAction>> GetListAsync(
        string jobId,
        bool? isEnabled = null, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.JobId.Equals(jobId))
            .WhereIf(isEnabled.HasValue, x => x.IsEnabled == isEnabled.Value)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
