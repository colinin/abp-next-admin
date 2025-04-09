using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;

public abstract class EfCoreDataProtectionRepository<TDbContext, TEntity, TKey, TEntityAuth> : 
    EfCoreRepository<TDbContext, TEntity, TKey>,
    IDataProtectionRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>, IDataProtected
    where TEntityAuth : DataAuthBase<TEntity, TKey>
{
    private readonly IDataAuthorizationService _dataAuthorizationService;
    private readonly IEntityTypeFilterBuilder _entityTypeFilterBuilder;
    private readonly IEntityPropertyResultBuilder _entityPropertyResultBuilder;

    protected ICurrentUser CurrentUser => LazyServiceProvider.GetRequiredService<ICurrentUser>();
    protected IDataAccessStrategyFilterBuilder StrategyFilterBuilder => LazyServiceProvider.GetService<IDataAccessStrategyFilterBuilder>();

    protected EfCoreDataProtectionRepository(
        [NotNull] IDbContextProvider<TDbContext> dbContextProvider,
        [NotNull] IDataAuthorizationService dataAuthorizationService,
        [NotNull] IEntityTypeFilterBuilder entityTypeFilterBuilder,
        [NotNull] IEntityPropertyResultBuilder entityPropertyResultBuilder)
        : base(dbContextProvider)
    {
        _dataAuthorizationService = dataAuthorizationService;
        _entityTypeFilterBuilder = entityTypeFilterBuilder;
        _entityPropertyResultBuilder = entityPropertyResultBuilder;
    }

    public async override Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();
        var queryable = dbSet.AsQueryable().AsNoTrackingIf(!ShouldTrackingEntityChange());

        var strategyFilterResult = await StrategyFilterBuilder?.Build<TEntity, TKey, TEntityAuth>(queryable, dbContext.Set<TEntityAuth>());
        if (strategyFilterResult != null && strategyFilterResult.Strategy != DataAccessStrategy.Custom)
        {
            // 根据配置的用户数据权限策略进行过滤
            queryable = strategyFilterResult.Queryable;
        }
        else
        {
            // 根据配置的用户实体数据权限规则过滤
            var dataAccessFilterExp = await _entityTypeFilterBuilder.Build<TEntity>(DataAccessOperation.Read);
            queryable = queryable.Where(dataAccessFilterExp);
        }

        // 仅查询授权字段
        var accessFieldExp = await _entityPropertyResultBuilder.Build<TEntity>(DataAccessOperation.Read);

        queryable = queryable.Select(accessFieldExp);

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
    

    public async override Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        CheckAndSetId(entity);

        var dbContext = await GetDbContextAsync();

        var savedEntity = (await dbContext.Set<TEntity>().AddAsync(entity, GetCancellationToken(cancellationToken))).Entity;

        await PersistenEntityAuthInfoAsync(dbContext, savedEntity, GetCancellationToken(cancellationToken));

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        return savedEntity;
    }

    public async override Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();
        if (entityArray.IsNullOrEmpty())
        {
            return;
        }

        var dbContext = await GetDbContextAsync();
        cancellationToken = GetCancellationToken(cancellationToken);

        foreach (var entity in entityArray)
        {
            CheckAndSetId(entity);
        }

        await PersistenEntityAuthInfoAsync(dbContext, entityArray, cancellationToken);

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.InsertManyAsync<TDbContext, TEntity>(
                this,
                entityArray,
                autoSave,
                GetCancellationToken(cancellationToken)
            );
            return;
        }

        await dbContext.Set<TEntity>().AddRangeAsync(entityArray, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
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

    /// <summary>
    /// 创建实体角色数据权限实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    protected virtual TEntityAuth CreateEntityRoleAuth(TEntity entity, string role)
    {
        var entityAuth = Activator.CreateInstance(typeof(TEntityAuth),
            new object[] { entity.Id, role, null, CurrentTenant.Id });

        return entityAuth as TEntityAuth;
    }
    /// <summary>
    /// 创建实体组织机构数据权限实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="ouCode"></param>
    /// <returns></returns>
    protected virtual TEntityAuth CreateEntityOrganizationUnitAuth(TEntity entity, string ouCode)
    {
        var entityAuth = Activator.CreateInstance(typeof(TEntityAuth),
            new object[] { entity.Id, null, ouCode, CurrentTenant.Id });

        return entityAuth as TEntityAuth;
    }
    /// <summary>
    /// 持久化实体数据权限
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async virtual Task PersistenEntityAuthInfoAsync(TDbContext dbContext, TEntity entity, CancellationToken cancellationToken = default)
    {
        // 可重写此方法以进行分布式持久化数据权限
        var entityAuth = dbContext.Set<TEntityAuth>();

        var entityRoleAuths = CurrentUser.Roles
            .Select((role) => CreateEntityRoleAuth(entity, role));
        var entityOuAuths = CurrentUser.FindOrganizationUnits()
            .Select((ouCode) => CreateEntityOrganizationUnitAuth(entity, ouCode));

        await entityAuth.AddRangeAsync(entityRoleAuths.Union(entityOuAuths), GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 持久化实体数据权限
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async virtual Task PersistenEntityAuthInfoAsync(TDbContext dbContext, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        // 可重写此方法以进行分布式持久化数据权限

        var entityAuth = dbContext.Set<TEntityAuth>();
        var entityAuths = new List<TEntityAuth>();

        foreach (var entity in entities)
        {
            var entityRoleAuths = CurrentUser.Roles
                .Select((role) => CreateEntityRoleAuth(entity, role));
            var entityOuAuths = CurrentUser.FindOrganizationUnits()
                .Select((ouCode) => CreateEntityOrganizationUnitAuth(entity, ouCode));

            entityAuths.AddRange(entityRoleAuths.Union(entityOuAuths));
        }

        await entityAuth.AddRangeAsync(entityAuths, GetCancellationToken(cancellationToken));
    }
}

public abstract class EfCoreDataProtectionRepository<TDbContext, TEntity> : EfCoreRepository<TDbContext, TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    private readonly IDataAuthorizationService _dataAuthorizationService;
    private readonly IEntityTypeFilterBuilder _entityTypeFilterBuilder;
    private readonly IEntityPropertyResultBuilder _entityPropertyResultBuilder;

    protected EfCoreDataProtectionRepository(
        [NotNull] IDbContextProvider<TDbContext> dbContextProvider,
        [NotNull] IDataAuthorizationService dataAuthorizationService,
        [NotNull] IEntityTypeFilterBuilder entityTypeFilterBuilder,
        [NotNull] IEntityPropertyResultBuilder entityPropertyResultBuilder) 
        : base(dbContextProvider)
    {
        _dataAuthorizationService = dataAuthorizationService;
        _entityTypeFilterBuilder = entityTypeFilterBuilder;
        _entityPropertyResultBuilder = entityPropertyResultBuilder;
    }

    public async override Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        var queryable = await base.GetQueryableAsync();

        var dataAccessFilterExp = await _entityTypeFilterBuilder.Build<TEntity>(DataAccessOperation.Read);
        var accessFieldExp = await _entityPropertyResultBuilder.Build<TEntity>(DataAccessOperation.Read);

        queryable = queryable.Where(dataAccessFilterExp).Select(accessFieldExp);

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