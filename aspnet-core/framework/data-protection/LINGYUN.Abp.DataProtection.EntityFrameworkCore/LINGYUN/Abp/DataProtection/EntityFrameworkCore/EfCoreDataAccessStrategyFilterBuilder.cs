using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;

public class EfCoreDataAccessStrategyFilterBuilder : DataAccessStrategyFilterBuilderBase
{
    private readonly ICurrentUser _currentUser;

    private static readonly MethodInfo LikeMethodInfo
        = typeof(DbFunctionsExtensions)
            .GetMethod(
                nameof(DbFunctionsExtensions.Like),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) });

    private static readonly MethodInfo ContainsMethodInfo
        = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(string));

    public EfCoreDataAccessStrategyFilterBuilder(
        ICurrentUser currentUser,
        IDataFilter dataFilter,
        IDataAccessScope dataAccessScope,
        IDataAccessStrategyStateProvider strategyStateProvider)
        : base(dataFilter, dataAccessScope, strategyStateProvider)
    {
        _currentUser = currentUser;
    }

    protected override IQueryable<TEntity> Build<TEntity, TKey, TEntityAuth>(IQueryable<TEntity> entity, IQueryable<TEntityAuth> entityAuth, DataAccessStrategyState state)
    {
        var parameterEntity = Expression.Parameter(typeof(TEntity), "e");
        var parameterEntityAuth = Expression.Parameter(typeof(TEntityAuth), "ea");

        var entityIdProperty = Expression.Property(parameterEntity, nameof(IEntity<TKey>.Id));
        var entityIdAuthProperty = Expression.Property(parameterEntityAuth, nameof(DataAuthBase<TEntity, TKey>.EntityId));

        // e.Id = ea.EntityId
        var entityIdMatch = Expression.Equal(entityIdAuthProperty, entityIdProperty);

        switch (state.Strategy)
        {
            case DataAccessStrategy.CurrentUser:
                var creatorIdProperty = Expression.Property(parameterEntity, nameof(IDataProtected.CreatorId));
                var creatorId = Expression.Constant(_currentUser.Id, typeof(Guid?));
                var currentUserExp = Expression.Equal(creatorIdProperty, creatorId);
                // e => e.CreatorId = _currentUser.Id
                var currentUserFunc = Expression.Lambda<Func<TEntity, bool>>(currentUserExp, parameterEntity);
                return entity.Where(currentUserFunc);

            case DataAccessStrategy.CurrentRoles:
                var roleProperty = Expression.Property(
                    parameterEntityAuth,
                    nameof(DataAuthBase<TEntity, TKey>.Role)
                );

                var roleContains = Expression.Call(
                    ContainsMethodInfo,
                    Expression.Constant(_currentUser.Roles),
                    roleProperty
                );

                // and ea.Role in ("Users") and e.Id = ea.EntityId
                var roleFinalCondition = Expression.AndAlso(roleContains, entityIdMatch);

                var existsSubQueryWithRole = Expression.Call(
                    typeof(Queryable),
                    "Any",
                    new Type[] { typeof(TEntityAuth) },
                    entityAuth.Expression,
                    Expression.Lambda<Func<TEntityAuth, bool>>(roleFinalCondition, parameterEntityAuth)
                );

                var whereExistsCondition = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { typeof(TEntity) },
                    entity.Expression,
                    Expression.Lambda<Func<TEntity, bool>>(
                        existsSubQueryWithRole, parameterEntity
                    )
                );
                return entity.Provider.CreateQuery<TEntity>(whereExistsCondition);
            case DataAccessStrategy.CurrentOrganizationUnits:
                var equalOrganizationUnits = _currentUser.FindOrganizationUnits();

                var ouProperty = Expression.Property(
                    parameterEntityAuth,
                    nameof(DataAuthBase<TEntity, TKey>.OrganizationUnit)
                );

                var ouContains = Expression.Call(
                    ContainsMethodInfo,
                    Expression.Constant(equalOrganizationUnits),
                    ouProperty
                );
                // and ea.OrganizationUnit in ("00001.00001") and e.Id = ea.EntityId
                var finalCondition = Expression.AndAlso(ouContains, entityIdMatch);

                var existsSubQueryWithOu = Expression.Call(
                    typeof(Queryable),
                    "Any",
                    new Type[] { typeof(TEntityAuth) },
                    entityAuth.Expression,
                    Expression.Lambda<Func<TEntityAuth, bool>>(finalCondition, parameterEntityAuth)
                );

                var whereExistsConditionWithOu = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { typeof(TEntity) },
                    entity.Expression,
                    Expression.Lambda<Func<TEntity, bool>>(
                        existsSubQueryWithOu, parameterEntity
                    )
                );

                return entity.Provider.CreateQuery<TEntity>(whereExistsConditionWithOu);

            case DataAccessStrategy.CurrentAndSubOrganizationUnits:
                var startsWithOrganizationUnits = _currentUser.FindOrganizationUnits();

                var dbFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));

                var filteredEntityAuth = startsWithOrganizationUnits
                   .Select(ouCode =>
                       (Expression)Expression.Call(
                           typeof(Queryable),
                           "Any",
                           new Type[] { typeof(TEntityAuth) },
                           entityAuth.Expression,
                           Expression.Lambda<Func<TEntityAuth, bool>>(
                               Expression.AndAlso(
                                   Expression.Equal(
                                       Expression.Property(parameterEntityAuth, nameof(DataAuthBase<TEntity, TKey>.EntityId)),
                                       Expression.Property(parameterEntity, nameof(IEntity<TKey>.Id))
                                   ),
                                   Expression.Call(
                                       LikeMethodInfo,
                                       dbFunctions,
                                       Expression.Property(parameterEntityAuth, nameof(DataAuthBase<TEntity, TKey>.OrganizationUnit)),
                                       Expression.Constant(ouCode + "%")
                                   )
                               ),
                               parameterEntityAuth
                           )
                       )
                   )
                   .Aggregate(Expression.OrElse);

                var startsWithWhereExistsConditionWithOu = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { typeof(TEntity) },
                    entity.Expression,
                    Expression.Lambda<Func<TEntity, bool>>(filteredEntityAuth, parameterEntity)
                );

                return entity.Provider.CreateQuery<TEntity>(startsWithWhereExistsConditionWithOu);
        }

        return entity;
    }
}
