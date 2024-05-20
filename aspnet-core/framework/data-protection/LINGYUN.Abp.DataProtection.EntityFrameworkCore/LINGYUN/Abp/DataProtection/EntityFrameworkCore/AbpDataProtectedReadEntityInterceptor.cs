using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;
public class AbpDataProtectedReadEntityInterceptor : IQueryExpressionInterceptor, ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;
    public IOptions<AbpDataProtectionOptions> DataProtectionOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpDataProtectionOptions>>();
    public ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();
    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();
    public IEntityTypeFilterBuilder EntityTypeFilterBuilder => LazyServiceProvider.LazyGetRequiredService<IEntityTypeFilterBuilder>();

    private static readonly MethodInfo WhereMethodInfo = typeof(Queryable).GetMethods().First(m => m.Name == nameof(Queryable.Where));

    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
    {
        if (DataFilter.IsEnabled<IDataProtected>() && queryExpression.Type.GenericTypeArguments.Length > 0)
        {
            var entityType = queryExpression.Type.GenericTypeArguments[0];
            var exp = EntityTypeFilterBuilder.Build(entityType, DataAccessOperation.Read);

            return Expression.Call(
                method: WhereMethodInfo.MakeGenericMethod(entityType),
                arg0: queryExpression,
                arg1: exp);
        }

        return queryExpression;
    }

    public class DataProtectedExpressionVisitor : ExpressionVisitor
    {
        private readonly Type _entityType;
        private readonly IEntityTypeFilterBuilder _entityTypeFilterBuilder;

        public DataProtectedExpressionVisitor(Type entityType, IEntityTypeFilterBuilder entityTypeFilterBuilder)
        {
            _entityType = entityType;
            _entityTypeFilterBuilder = entityTypeFilterBuilder;
        }

        private static readonly MethodInfo WhereMethodInfo = typeof(Queryable).GetMethods().First(m => m.Name == nameof(Queryable.Where));

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            var method = WhereMethodInfo.MakeGenericMethod(_entityType);
            var args0 = base.VisitMethodCall(methodCallExpression);
            var args1 = _entityTypeFilterBuilder.Build(_entityType, DataAccessOperation.Read);

            return Expression.Call(
                method: method,
                arg0: args0,
                arg1: args1);
        }
    }
}
