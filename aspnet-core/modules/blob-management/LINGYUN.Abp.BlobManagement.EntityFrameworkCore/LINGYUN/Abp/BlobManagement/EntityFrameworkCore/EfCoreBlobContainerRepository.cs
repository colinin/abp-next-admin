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

namespace LINGYUN.Abp.BlobManagement.EntityFrameworkCore;

public class EfCoreBlobContainerRepository : EfCoreRepository<IBlobManagementDbContext, BlobContainer, Guid>, IBlobContainerRepository
{
    public EfCoreBlobContainerRepository(
        IDbContextProvider<IBlobManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<string> GetNameAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.Id == id)
            .Select(x => x.Name)
            .FirstAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<BlobContainer?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<BlobContainer> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<BlobContainer>> GetListAsync(
        ISpecification<BlobContainer> specification,
        string? sorting = nameof(BlobContainer.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(BlobContainer.Name))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
