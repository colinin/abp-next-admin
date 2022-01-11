using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

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

    public virtual async Task<int> GetCountAsync(
        BackgroundJobLogFilter filter,
        Guid? jobId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(jobId.HasValue, x => x.JobId.Equals(jobId))
            .WhereIf(!filter.Type.IsNullOrWhiteSpace(), x => x.JobType.Contains(filter.Type))
            .WhereIf(!filter.Group.IsNullOrWhiteSpace(), x => x.JobGroup.Equals(filter.Group))
            .WhereIf(!filter.Name.IsNullOrWhiteSpace(), x => x.JobName.Equals(filter.Name))
            .WhereIf(!filter.Filter.IsNullOrWhiteSpace(), x => x.JobName.Contains(filter.Filter) ||
                x.JobGroup.Contains(filter.Filter) || x.JobType.Contains(filter.Filter) || x.Message.Contains(filter.Filter))
            .WhereIf(filter.HasExceptions.HasValue, x => !string.IsNullOrWhiteSpace(x.Exception))
            .WhereIf(filter.BeginRunTime.HasValue, x => x.RunTime.CompareTo(filter.BeginRunTime.Value) >= 0)
            .WhereIf(filter.EndRunTime.HasValue, x => x.RunTime.CompareTo(filter.EndRunTime.Value) <= 0)
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<BackgroundJobLog>> GetListAsync(
        BackgroundJobLogFilter filter,
        Guid? jobId = null,
        string sorting = nameof(BackgroundJobLog.RunTime),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(jobId.HasValue, x => x.JobId.Equals(jobId))
            .WhereIf(!filter.Type.IsNullOrWhiteSpace(), x => x.JobType.Contains(filter.Type))
            .WhereIf(!filter.Group.IsNullOrWhiteSpace(), x => x.JobGroup.Equals(filter.Group))
            .WhereIf(!filter.Name.IsNullOrWhiteSpace(), x => x.JobName.Equals(filter.Name))
            .WhereIf(!filter.Filter.IsNullOrWhiteSpace(), x => x.JobName.Contains(filter.Filter) ||
                x.JobGroup.Contains(filter.Filter) || x.JobType.Contains(filter.Filter) || x.Message.Contains(filter.Filter))
            .WhereIf(filter.HasExceptions.HasValue, x => !string.IsNullOrWhiteSpace(x.Exception))
            .WhereIf(filter.BeginRunTime.HasValue, x => x.RunTime.CompareTo(filter.BeginRunTime.Value) >= 0)
            .WhereIf(filter.EndRunTime.HasValue, x => x.RunTime.CompareTo(filter.EndRunTime.Value) <= 0)
            .OrderBy(sorting ?? nameof(BackgroundJobInfo.Name))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
