using LINGYUN.Abp.DataProtection;
using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;
/// <summary>
/// 实现基本接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">实体主键类型</typeparam>
public abstract class EfCoreProjectNameRepository<TEntity, TKey> :
    EfCoreDataProtectionRepository<IProjectNameDbContext, TEntity, TKey>,
    IProjectNameBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected EfCoreProjectNameRepository(
        IDbContextProvider<IProjectNameDbContext> dbContextProvider, 
        IDataAuthorizationService dataAuthorizationService, 
        IEntityTypeFilterBuilder entityTypeFilterBuilder) 
        : base(dbContextProvider, dataAuthorizationService, entityTypeFilterBuilder)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<TEntity> specification, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<TEntity>> GetListAsync(
        ISpecification<TEntity> specification,
        string sorting = nameof(IEntity<TKey>.Id), 
        int maxResultCount = 10,
        int skipCount = 0, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
              .Where(specification.ToExpression())
              .OrderBy(GetSortingOrDefault(sorting))
              .PageBy(skipCount, maxResultCount)
              .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<TEntity>> GetListAsync(
        ISpecification<TEntity> specification,
        string sorting = nameof(IEntity<TKey>.Id),
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
              .Where(specification.ToExpression())
              .OrderBy(GetSortingOrDefault(sorting))
              .Take(maxResultCount)
              .ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual string GetSortingOrDefault(string sorting = nameof(IEntity<TKey>.Id))
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            return nameof(IEntity<TKey>.Id);
        }
        return sorting;
    }
}
