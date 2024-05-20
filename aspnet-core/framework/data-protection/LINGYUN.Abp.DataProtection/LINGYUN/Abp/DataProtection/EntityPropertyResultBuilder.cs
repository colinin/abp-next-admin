using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.DataProtection;
public class EntityPropertyResultBuilder : IEntityPropertyResultBuilder, ITransientDependency
{
    private readonly IDataFilter _dataFilter;
    private readonly IServiceProvider _serviceProvider;
    private readonly AbpDataProtectionOptions _options;

    public EntityPropertyResultBuilder(
        IDataFilter dataFilter,
        IServiceProvider serviceProvider,
        IOptions<AbpDataProtectionOptions> options)
    {
        _options = options.Value;
        _dataFilter = dataFilter;
        _serviceProvider = serviceProvider;
    }

    public virtual LambdaExpression Build(Type entityType, DataAccessOperation operation)
    {
        // Func<TEntity, TEntity>
        var func = typeof(Func<,>).MakeGenericType(entityType, entityType);
        var param = Expression.Parameter(entityType, "e");
        LambdaExpression selector = Expression.Lambda(param, param);
        if (!ShouldApplyFilter(entityType, operation))
        {
            return selector;
        }

        var typeName = entityType.FullName;
        var allowProperties = new List<string>();
        var subjectContext = new DataAccessSubjectContributorContext(typeName, operation, _serviceProvider);
        foreach (var contributor in _options.SubjectContributors)
        {
            var properties = contributor.GetAllowProperties(subjectContext);
            allowProperties.AddIfNotContains(properties);
        }

        // 默认实体相关标识需要带上, 否则无法返回查询结果
        allowProperties.AddIfNotContains(_options.IgnoreAuditedProperties);

        if (!allowProperties.Any())
        {
            return selector;
        }

        var memberBindings = new List<MemberBinding>();
        foreach (var propertyName in allowProperties)
        {
            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var propertyExpression = Expression.Property(param, propertyInfo);
                memberBindings.Add(Expression.Bind(propertyInfo, propertyExpression));
            }
        }
        var newExpression = Expression.New(entityType);
        var memberInitExpression = Expression.MemberInit(newExpression, memberBindings);

        return Expression.Lambda(func, memberInitExpression, param);
    }

    public virtual Expression<Func<TEntity, TEntity>> Build<TEntity>(DataAccessOperation operation)
    {
        var entityType = typeof(TEntity);
        Expression<Func<TEntity, TEntity>> selector = e => e;

        if (!ShouldApplyFilter(entityType, operation))
        {
            return selector;
        }

        var typeName = typeof(TEntity).FullName;
        var allowProperties = new List<string>();
        var subjectContext = new DataAccessSubjectContributorContext(typeName, operation, _serviceProvider);
        foreach (var contributor in _options.SubjectContributors)
        {
            var properties = contributor.GetAllowProperties(subjectContext);
            allowProperties.AddIfNotContains(properties);
        }

        // 默认实体相关标识需要带上, 否则无法返回查询结果
        allowProperties.AddIfNotContains(_options.IgnoreAuditedProperties);

        if (!allowProperties.Any())
        {
            return selector;
        }

        var param = Expression.Parameter(typeof(TEntity), "e");
        var memberBindings = new List<MemberBinding>();
        foreach (var propertyName in allowProperties)
        {
            var propertyInfo = typeof(TEntity).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var propertyExpression = Expression.Property(param, propertyInfo);
                memberBindings.Add(Expression.Bind(propertyInfo, propertyExpression));
            }
        }
        var newExpression = Expression.New(typeof(TEntity));
        var memberInitExpression = Expression.MemberInit(newExpression, memberBindings);

        return Expression.Lambda<Func<TEntity, TEntity>>(memberInitExpression, param);
    }

    private bool ShouldApplyFilter(Type entityType, DataAccessOperation operation)
    {
        if (!_dataFilter.IsEnabled<IDataProtected>())
        {
            return false;
        }

        if (entityType.IsDefined(typeof(DisableDataProtectedAttribute), true))
        {
            return false;
        }

        var dataProtected = entityType.GetCustomAttribute<DataProtectedAttribute>();

        if (dataProtected != null && dataProtected.Operation != operation)
        {
            return false;
        }

        return true;
    }
}
