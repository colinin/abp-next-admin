using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;

public abstract class EfCoreDataProtectionRepository<TDbContext, TEntity, TKey> : 
    EfCoreRepository<TDbContext, TEntity, TKey>,
    IDataProtectionRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
{
    private readonly IDataAuthorizationService _dataAuthorizationService;
    private readonly IEntityTypeFilterBuilder _entityTypeFilterBuilder;

    protected EfCoreDataProtectionRepository(
        [NotNull] IDbContextProvider<TDbContext> dbContextProvider,
        [NotNull] IDataAuthorizationService dataAuthorizationService,
        [NotNull] IEntityTypeFilterBuilder entityTypeFilterBuilder)
        : base(dbContextProvider)
    {
        _dataAuthorizationService = dataAuthorizationService;
        _entityTypeFilterBuilder = entityTypeFilterBuilder;
    }

    public async override Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        var queryable = await base.GetQueryableAsync();

        var dataAccessFilterExp = _entityTypeFilterBuilder.Build<TEntity>(DataAccessOperation.Read);
        queryable = queryable.Where(dataAccessFilterExp);

        return queryable;
    }

    public async override Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();
        var entities = await dbSet.Where(predicate).ToListAsync();

        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Delete, entities);

        await dbSet.Where(predicate).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Delete, new TEntity[1] { entity });

        await base.DeleteAsync(entity, autoSave, cancellationToken);
    }

    public async override Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Delete, entities);

        await base.DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    public async override Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Write, new TEntity[1] { entity });

        return await base.UpdateAsync(entity, autoSave, cancellationToken);
    }

    public async override Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Write, entities);

        await base.UpdateManyAsync(entities, autoSave, cancellationToken);
    }
}

public abstract class EfCoreDataProtectionRepository<TDbContext, TEntity> : EfCoreRepository<TDbContext, TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    private readonly IDataAuthorizationService _dataAuthorizationService;
    private readonly IEntityTypeFilterBuilder _entityTypeFilterBuilder;

    protected EfCoreDataProtectionRepository(
        [NotNull] IDbContextProvider<TDbContext> dbContextProvider,
        [NotNull] IDataAuthorizationService dataAuthorizationService,
        [NotNull] IEntityTypeFilterBuilder entityTypeFilterBuilder) 
        : base(dbContextProvider)
    {
        _dataAuthorizationService = dataAuthorizationService;
        _entityTypeFilterBuilder = entityTypeFilterBuilder;
    }

    public async override Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        var queryable = await base.GetQueryableAsync();

        var dataAccessFilterExp = _entityTypeFilterBuilder.Build<TEntity>(DataAccessOperation.Read);
        queryable = queryable.Where(dataAccessFilterExp);

        return queryable;
    }

    public async override Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();
        var entities = await dbSet.Where(predicate).ToListAsync();

        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Delete, entities);

        await dbSet.Where(predicate).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Delete, new TEntity[1] { entity });

        await base.DeleteAsync(entity, autoSave, cancellationToken);
    }

    public async override Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, 
        CancellationToken cancellationToken = default)
    {
        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Delete, entities);

        await base.DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    public async override Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, 
        CancellationToken cancellationToken = default)
    {
        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Write, new TEntity[1] { entity });

        return await base.UpdateAsync(entity, autoSave, cancellationToken);
    }

    public async override Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await _dataAuthorizationService.CheckAsync(DataAccessOperation.Write, entities);

        await base.UpdateManyAsync(entities, autoSave, cancellationToken);
    }
}