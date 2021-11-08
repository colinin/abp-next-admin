using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore
{
    /// <summary>
    /// 受保护的资源仓储接口需要继承此接口,否则需要自行实现过滤器
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EfCoreDataProtectionRepositoryBase<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity, TKey>
        where TEntity : class, IEntity<TKey>, IDataProtection
        where TDbContext: IEfCoreDbContext
    {
        protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetService<ICurrentUser>();
        protected IDataProtectdChecker DataProtectdChecker => LazyServiceProvider.LazyGetService<IDataProtectdChecker>();
        protected EfCoreDataProtectionRepositoryBase(
            IDbContextProvider<TDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (CurrentUser.IsAuthenticated && 
                entity is IDataProtection protectedEntity)
            {
                ProtectedEntityHelper.TrySetOwner(
                    protectedEntity,
                    () => string.Join(",", CurrentUser.UserName, CurrentUser.Roles.JoinAsString(",")));
            }

            return await base.InsertAsync(entity, autoSave, cancellationToken);
        }

        public override async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (CurrentUser.IsAuthenticated && 
                typeof(IDataProtection).IsAssignableFrom(typeof(TEntity)))
            {
                foreach (var entity in entities)
                {
                    ProtectedEntityHelper.TrySetOwner(
                        entity,
                        () => string.Join(",", CurrentUser.UserName, CurrentUser.Roles.JoinAsString(",")));
                }
            }

            await base.InsertManyAsync(entities, autoSave, cancellationToken);
        }

        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return includeDetails
                   ? await(await WithDetailsAsync(ProtectBehavior.Query)).ToListAsync(GetCancellationToken(cancellationToken))
                   : await(await GetQueryableAsync(ProtectBehavior.Query)).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return includeDetails
                 ? await(await WithDetailsAsync(ProtectBehavior.Query)).OrderBy(e => e.Id).FirstOrDefaultAsync(e => e.Id.Equals(id), GetCancellationToken(cancellationToken))
                 : await(await GetQueryableAsync(ProtectBehavior.Query)).OrderBy(e => e.Id).FirstOrDefaultAsync(e => e.Id.Equals(id), GetCancellationToken(cancellationToken));
        }

        public override async Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate, 
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            return includeDetails
                 ? await (await WithDetailsAsync(ProtectBehavior.Query)).Where(predicate).ToListAsync(GetCancellationToken(cancellationToken))
                 : await (await GetQueryableAsync(ProtectBehavior.Query)).Where(predicate).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync(ProtectBehavior.Delete);
            var entity = await queryable
                .FirstOrDefaultAsync(e => e.Id.Equals(id), GetCancellationToken(cancellationToken));
            if (entity == null)
            {
                return;
            }
            await DeleteAsync(entity, autoSave, cancellationToken);
        }

        public override async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync(ProtectBehavior.Delete);

            var entities = await queryable.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }

        public override async Task DeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync(ProtectBehavior.Delete);

            var entities = await queryable.Where(predicate).ToListAsync(cancellationToken);

            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }
        protected virtual async Task<IQueryable<TEntity>> WithDetailsAsync(ProtectBehavior behavior = ProtectBehavior.All)
        {
            if (typeof(IDataProtection).IsAssignableFrom(typeof(TEntity)))
            {
                var result = await DataProtectdChecker.IsGrantedAsync<TEntity>(behavior);
                if (!result.Succeeded)
                {
                    var queryable = await base.GetQueryableAsync();
                    return queryable.Where((t) => false);
                }

                return await WithDetailsAsync(result);
            }

            return await base.WithDetailsAsync();
        }
        protected virtual async Task<IQueryable<TEntity>> WithDetailsAsync(ResourceGrantedResult resourceGranted)
        {
            if (AbpEntityOptions.DefaultWithDetailsFunc == null)
            {
                return await GetQueryableAsync(resourceGranted);
            }

            return AbpEntityOptions.DefaultWithDetailsFunc(await GetQueryableAsync(resourceGranted));
        }
        protected virtual async Task<IQueryable<TEntity>> GetQueryableAsync(ProtectBehavior behavior = ProtectBehavior.All)
        {
            if (typeof(IDataProtection).IsAssignableFrom(typeof(TEntity)))
            {
                var result = await DataProtectdChecker.IsGrantedAsync<TEntity>(behavior);
                if (!result.Succeeded)
                {
                    var queryable = await base.GetQueryableAsync();
                    return queryable.Where((t) => false);
                }

                return await GetQueryableAsync(result);
            }

            return await base.GetQueryableAsync();
        }
        protected virtual async Task<IQueryable<TEntity>> GetQueryableAsync(ResourceGrantedResult resourceGranted)
        {
            var queryable = await base.GetQueryableAsync();
            if (!resourceGranted.Succeeded)
            {
                // 无资源访问权限, 不返回结果
                return queryable.Where((t) => false);
            }
            // 资源过滤,用户是否有对某个资源的访问权限
            // 方案1、Resource.Owner In ("user1", "user2", "role1", "role2", "organization1", "...")     独立模块，业务增加Owner字段
            // 方案2、Select R.* From Resource R Inner Join Protect T On T.Visitor = R.Owner Where T.Resource = 'Resource'   业务侵入,增加Protect表
            queryable = FilterResource(queryable, resourceGranted.Resource);
            // 对于可访问资源的进一步动态规则过滤      1 == 1 And Resource.Field1 = 'allow' And Resource.Field2 >= 100 And Resource.Field2 <= 200
            queryable = FilterFieldRule(queryable, resourceGranted.Rules);
            // 对于资源可访问字段过滤                  Select Resource.Field1, Resource.Field2, Resource.Field3
            queryable = FilterFieldReturn(queryable, resourceGranted.Fields);

            return queryable;
        }

        protected virtual IQueryable<T> FilterResource<T>(IQueryable<T> queryable, ProtectedResource resource)
            where T : IDataProtection
        {
            ParameterExpression pe = Expression.Parameter(typeof(T));

            // 检查资源的可访问者
            // any: 内置常量,允许访问所有资源
            if (!resource.Visitor.IsNullOrWhiteSpace() || !resource.Visitor.Contains("any"))
            {
                // 过滤允许的资源访问者
                // 方案一：模块之间独立，传递当前访问者即可
                // Select * From Resource As R Where R.Owner LIKE ('visitor1', 'visitorRole1')
                var ownerExp = Expression.PropertyOrField(pe, nameof(IDataProtection.Owner));
                var visities = resource.Visitor.Split(',');
                Expression visitorExpr = null;
                foreach (var visitor in visities)
                {
                    visitorExpr = visitorExpr == null
                        ? Expression.Call(
                            ownerExp,
                            typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                            Expression.Constant(visitor, ownerExp.Type))
                        : Expression.Or(
                            visitorExpr,
                            Expression.Call(
                                ownerExp,
                                typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                                Expression.Constant(visitor, ownerExp.Type)));
                }

                // 方案二：节省网络带宽,快速查询
                // Select R.* From Resource As R
                // Inner Join Protect As P On P.Resource = 'Resource'
                // Where 1 == 1
                // And P.Behavior = ProtectBehavior.Query
                // And ((P.Visitor = 'visitor1') Or (P.Visitor = 'visitorRole1') Or (P.Visitor = 'visitorRole2'))
                queryable = queryable.Where(Expression.Lambda<Func<T, bool>>(visitorExpr, pe));
            }

            return queryable;
        }

        protected virtual IQueryable<T> FilterFieldRule<T>(IQueryable<T> queryable, IEnumerable<ProtectedFieldRule> rules)
            where T : IDataProtection
        {
            ParameterExpression pe = Expression.Parameter(typeof(T));

            // 默认未指定访问规则
            // 则可访问所有允许的资源
            if (rules.Any())
            {
                Expression<Func<T, bool>> where = PredicateBuilder.New<T>((t) => true);
                foreach (var fieldRule in rules)
                {
                    var memberExp = Expression.PropertyOrField(pe, fieldRule.Field);
                    Expression memberCondition = null;
                    memberCondition = fieldRule.Operator switch
                    {
                        // LIKE
                        ExpressionType.Contains => Expression.Call(
                                                        memberExp,
                                                        typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                                                        Expression.Constant(fieldRule.Value, memberExp.Type)),
                        // ==
                        ExpressionType.Equal => Expression.Equal(memberExp, Expression.Constant(fieldRule.Value, memberExp.Type)),
                        // <
                        ExpressionType.LessThan => Expression.LessThan(memberExp, Expression.Constant(fieldRule.Value, memberExp.Type)),
                        // <=
                        ExpressionType.LessThanOrEqual => Expression.LessThanOrEqual(memberExp, Expression.Constant(fieldRule.Value, memberExp.Type)),
                        // >
                        ExpressionType.GreaterThan => Expression.GreaterThan(memberExp, Expression.Constant(fieldRule.Value, memberExp.Type)),
                        // >=
                        ExpressionType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(memberExp, Expression.Constant(fieldRule.Value, memberExp.Type)),
                        // 其他操作符未引入
                        _ => throw new NotSupportedException($"Dynamic rules do not support operator: {fieldRule.Operator}"),
                    };
                    switch (fieldRule.Logic)
                    {
                        case PredicateOperator.And:
                            where = where.And(Expression.Lambda<Func<T, bool>>(memberCondition, pe));
                            break;
                        case PredicateOperator.Or:
                            where = where.Or(Expression.Lambda<Func<T, bool>>(memberCondition, pe));
                            break;
                    }
                }
                queryable = queryable.Where(where);
            }

            return queryable;
        }

        protected virtual IQueryable<T> FilterFieldReturn<T>(IQueryable<T> queryable, IEnumerable<ProtectedField> fields)
        {
            // 默认未指定可访问字段则返回所有字段
            if (fields.Any())
            {
                ParameterExpression pe = Expression.Parameter(typeof(T));
                Type queryableResultType = typeof(T);
                NewExpression ne = Expression.New(queryableResultType);
                List<MemberAssignment> members = new List<MemberAssignment>();

                foreach (var field in fields)
                {
                    var fieldProp = queryableResultType.GetProperty(field.Field);
                    var meField = Expression.MakeMemberAccess(pe, fieldProp);
                    members.Add(Expression.Bind(fieldProp, meField));
                }

                var mie = Expression.MemberInit(ne, members);
                Expression<Func<T, T>> personSelectExpression = Expression.Lambda<Func<T, T>>(mie, pe);

                queryable = queryable.Select(personSelectExpression);
            }

            return queryable;
        }
    }
}
