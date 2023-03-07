using LINGYUN.Abp.BackgroundTasks;
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
using Volo.Abp.Timing;

namespace LINGYUN.Abp.TaskManagement.EntityFrameworkCore;

public class EfCoreBackgroundJobInfoRepository :
    EfCoreRepository<TaskManagementDbContext, BackgroundJobInfo, string>,
    IBackgroundJobInfoRepository
{
    protected IClock Clock { get; }

    public EfCoreBackgroundJobInfoRepository(
        IClock clock,
        IDbContextProvider<TaskManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
        Clock = clock;
    }

    public async virtual Task<bool> CheckNameAsync(
        string group,
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(x => x.Group.Equals(group) && x.Name.Equals(name),
                GetCancellationToken(cancellationToken));
    }

    public async virtual Task<JobInfo> FindJobAsync(
        string id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Id.Equals(id))
            .Select(x => x.ToJobInfo())
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<BackgroundJobInfo>> GetExpiredJobsAsync(
        int maxResultCount,
        TimeSpan jobExpiratime,
        CancellationToken cancellationToken = default)
    {
        var expiratime = Clock.Now.Subtract(jobExpiratime);

        return await (await GetDbSetAsync())
            .Where(x => x.Status == JobStatus.Completed && x.LastRunTime <= expiratime)
            .OrderBy(x => x.CreationTime)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<BackgroundJobInfo>> GetAllPeriodTasksAsync(
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(new BackgroundJobInfoWaitingPeriodSpecification().ToExpression())
            .OrderByDescending(x => x.Priority)
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(ISpecification<BackgroundJobInfo> specification, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<BackgroundJobInfo>> GetListAsync(ISpecification<BackgroundJobInfo> specification, string sorting = "Name", int maxResultCount = 10, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting ?? $"{nameof(BackgroundJobInfo.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<BackgroundJobInfo>> GetWaitingListAsync(
        int maxResultCount,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(new BackgroundJobInfoWaitingSpecification().ToExpression())
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.TryCount)
            .ThenBy(x => x.NextRunTime)
            .Take(maxResultCount)
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
