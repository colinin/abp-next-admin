using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

public abstract class EfCoreProjectNameBasicRepository<TEntity, TKey> :
    EfCoreRepository<IProjectNameDbContext, TEntity, TKey>,
    IProjectNameBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected EfCoreProjectNameBasicRepository(
        IDbContextProvider<IProjectNameDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<TEntity>> GetListAsync(
        ISpecification<TEntity> specification,
        string? sorting = nameof(IEntity<TKey>.Id),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
              .Where(specification.ToExpression())
              .OrderBy(GetSortingOrDefault(sorting))
              .PageBy(skipCount, maxResultCount)
              .ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual string GetSortingOrDefault(string? sorting = nameof(IEntity<TKey>.Id))
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            return nameof(IEntity<TKey>.Id);
        }
        return sorting;
    }
}
