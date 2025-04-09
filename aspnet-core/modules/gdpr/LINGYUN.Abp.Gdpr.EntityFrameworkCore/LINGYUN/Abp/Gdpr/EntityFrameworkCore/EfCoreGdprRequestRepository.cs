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

namespace LINGYUN.Abp.Gdpr.EntityFrameworkCore;

public class EfCoreGdprRequestRepository(IDbContextProvider<IGdprDbContext> dbContextProvider) : 
    EfCoreRepository<IGdprDbContext, GdprRequest, Guid>(dbContextProvider), 
    IGdprRequestRepository
{
    public async virtual Task<DateTime?> FindLatestRequestTimeAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreationTime)
            .Select(x => x.CreationTime)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(ISpecification<GdprRequest> specification, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<GdprRequest>> GetListAsync(
        ISpecification<GdprRequest> specification,
        string? sorting = $"{nameof(GdprRequest.CreationTime)} DESC",
        int maxResultCount = 10, 
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await(await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(GdprRequest.CreationTime)} DESC" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<IQueryable<GdprRequest>> WithDetailsAsync()
    {
        return (await base.WithDetailsAsync()).Include(x => x.Infos);
    }
}
