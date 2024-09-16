using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.TaskManagement.EntityFrameworkCore;

public class EfCoreBackgroundJobLogRepository :
    EfCoreRepository<TaskManagementDbContext, BackgroundJobLog, long>,
    IBackgroundJobLogRepository
{
    public EfCoreBackgroundJobLogRepository(
        IDbContextProvider<TaskManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<BackgroundJobLog> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<BackgroundJobLog>> GetListAsync(
        ISpecification<BackgroundJobLog> specification,
        string sorting = $"{nameof(BackgroundJobLog.RunTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{nameof(BackgroundJobLog.RunTime)} DESC";
        }
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
