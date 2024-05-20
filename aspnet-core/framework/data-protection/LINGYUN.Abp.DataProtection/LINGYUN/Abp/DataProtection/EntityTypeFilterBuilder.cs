using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection;

public class EntityTypeFilterBuilder : IEntityTypeFilterBuilder, ITransientDependency
{
    private readonly IDataFilter _dataFilter;
    private readonly IServiceProvider _serviceProvider;
    private readonly AbpDataProtectionOptions _options;

    public EntityTypeFilterBuilder(
        IDataFilter dataFilter,
        IServiceProvider serviceProvider,
        IOptions<AbpDataProtectionOptions> options)
    {
        _options = options.Value;
        _dataFilter = dataFilter;
        _serviceProvider = serviceProvider;
    }

    public virtual LambdaExpression Build(Type entityType, DataAccessOperation operation, DataAccessFilterGroup group = null)
    {
        // Func<TEntity, bool>
        var func = typeof(Func<,>).MakeGenericType(entityType, typeof(bool));
        // e => e
        var param = Expression.Parameter(entityType, "e");
        // => true
        var trueExp = Expression.Constant(true);
        //  Expression<Func<TEntity, bool>> exp = e => true;
        var exp = Expression.Lambda(func, trueExp, param);

        if (!ShouldApplyFilter(entityType, operation))
        {
            return exp;
        }

        if (group != null)
        {
            exp = GetExpression(entityType, group);
        }

        var typeName = entityType.FullName;
        var subjectFilterGroups = new List<DataAccessFilterGroup>();
        var subjectContext = new DataAccessSubjectContributorContext(typeName, operation, _serviceProvider);
        foreach (var contributor in _options.SubjectContributors)
        {
            var subjectFilterGroup = contributor.GetFilterGroups(subjectContext);
            subjectFilterGroups.AddRange(subjectFilterGroup);
        }

        LambdaExpression subExp = null;
        foreach (var subGroup in subjectFilterGroups)
        {
            subExp = subExp == null ? GetExpression(entityType, subGroup) : subExp.OrElse(func, GetExpression(entityType, subGroup));
        }

        if (subExp == null)
        {
            return exp;
        }

        if (group == null)
        {
            return subExp;
        }
        exp = subExp.AndAlso(func, exp);

        return exp;
    }

    public virtual Expression<Func<TEntity, bool>> Build<TEntity>(DataAccessOperation operation, DataAccessFilterGroup group = null)
    {
        var entityType = typeof(TEntity);
        Expression<Func<TEntity, bool>> exp = _ => true;

        if (!ShouldApplyFilter(entityType, operation))
        {
            return exp;
        }

        if (group != null)
        {
            exp = GetExpression<TEntity>(group);
        }

        var typeName = typeof(TEntity).FullName;
        var subjectFilterGroups = new List<DataAccessFilterGroup>();
        var subjectContext = new DataAccessSubjectContributorContext(typeName, operation, _serviceProvider);
        foreach (var contributor in _options.SubjectContributors)
        {
            var subjectFilterGroup = contributor.GetFilterGroups(subjectContext);
            subjectFilterGroups.AddRange(subjectFilterGroup);
        }

        Expression<Func<TEntity, bool>> subExp = null;
        foreach ( var subGroup in subjectFilterGroups)
        {
            subExp = subExp == null ? GetExpression<TEntity>(subGroup) : subExp.Or(GetExpression<TEntity>(subGroup));
        }

        if (subExp == null)
        {
            return exp;
        }

        if (group == null)
        {
            return subExp;
        }
        exp = subExp.And(exp);

        return exp;
    }

    protected virtual Expression<Func<TEntity, bool>> GetExpression<TEntity>(DataAccessFilterGroup group)
    {
        Check.NotNull(group, nameof(group));
            
        var param = Expression.Parameter(typeof(TEntity), "e");
        var body = GetExpressionBody(param, group);
        var expression = Expression.Lambda<Func<TEntity, bool>>(body, param);
        return expression;
    }

    protected virtual LambdaExpression GetExpression(Type entityType, DataAccessFilterGroup group)
    {
        Check.NotNull(group, nameof(group));

        var func = typeof(Func<,>).MakeGenericType(entityType, typeof(bool));
        var param = Expression.Parameter(entityType, "e");
        var body = GetExpressionBody(param, group);
        var expression = Expression.Lambda(func, body, param);
        return expression;
    }

    protected virtual Expression<Func<TEntity, bool>> GetExpression<TEntity>(DataAccessFilterRule rule)
    {
        Check.NotNull(rule, nameof(rule));
            
        var param = Expression.Parameter(typeof(TEntity), "e");
        var body = GetExpressionBody(param, rule);
        var expression = Expression.Lambda<Func<TEntity, bool>>(body, param);
        return expression;
    }

    protected virtual LambdaExpression GetExpression(Type entityType, DataAccessFilterRule rule)
    {
        Check.NotNull(rule, nameof(rule));

        var func = typeof(Func<,>).MakeGenericType(entityType, typeof(bool));
        var param = Expression.Parameter(entityType, "e");
        var body = GetExpressionBody(param, rule);

        var expression = Expression.Lambda(func, body, param);
        return expression;
    }

    private Expression GetExpressionBody(ParameterExpression param, DataAccessFilterGroup group)
    {
        Check.NotNull(param, nameof(param));

        if (group == null || (!group.Rules.Any() && !group.Groups.Any()))
        {
            return Expression.Constant(true);
        }
        var bodies = new List<Expression>();
        bodies.AddRange(group.Rules.Select(rule => GetExpressionBody(param, rule)));
        bodies.AddRange(group.Groups.Select(subGroup => GetExpressionBody(param, subGroup)));

        return group.Logic switch {
            DataAccessFilterLogic.And => bodies.Aggregate(Expression.AndAlso),
            DataAccessFilterLogic.Or => bodies.Aggregate(Expression.OrElse),
            _ => throw new InvalidOperationException($"Not allowed filter logic: {group.Logic}")
        };
    }

    private Expression GetExpressionBody(ParameterExpression param, DataAccessFilterRule rule)
    {
        if (!_options.OperateContributors.TryGetValue(rule.Operate, out var contributor))
        {
            throw new InvalidOperationException($"Invalid data permission operator {rule.Operate}");
        }
        if (rule == null)
        {
            return Expression.Constant(true);
        }
        var expression = GetPropertyLambdaExpression(param, rule);
        if (expression == null)
        {
            return Expression.Constant(true);
        }
        var constant = ChangeTypeToExpression(rule, expression.Body.Type);

        return rule.IsLeft ? contributor.BuildExpression(constant, expression.Body) : contributor.BuildExpression(expression.Body, constant);
    }

    private static LambdaExpression GetPropertyLambdaExpression(ParameterExpression param, DataAccessFilterRule rule)
    {
        var propertyNames = rule.Field.Split('.');
        Expression propertyAccess = param;
        var type = param.Type;
        for (var index = 0; index < propertyNames.Length; index++)
        {
            var propertyName = propertyNames[index];

            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException(
                    $"The specified property '{rule.Field}' does not exist in type '{type.FullName}'");
            }

            type = property.PropertyType;
            if (index == propertyNames.Length - 1)
            {
                if (!CheckFilterRule(type, rule))
                {
                    return null;
                }
            }

            propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
        }
        return Expression.Lambda(propertyAccess, param);
    }
    
    private Expression ChangeTypeToExpression(DataAccessFilterRule rule, Type conversionType)
    {
        if (_options.KeywordContributors.TryGetValue(rule.Value?.ToString() ?? "", out var contributor))
        {
            var context = new DataAccessKeywordContributorContext(_serviceProvider);
            var keyValue = contributor.Contribute(context);
            var value = CastTo(keyValue, conversionType);
            return Expression.Constant(value, conversionType);
        }
        else
        {
            if (rule.Value is JsonElement element)
            {
                var valueArray = Array.CreateInstance(conversionType, element.GetArrayLength());
                var index = 0;
                foreach (var node in element.EnumerateArray())
                {
                    var value = CastTo(node.ToString(), conversionType);
                    valueArray.SetValue(value, index);
                    index++;
                }
                var arrayType = typeof(IEnumerable<>).MakeGenericType(conversionType);

                return Expression.Constant(valueArray, arrayType);
            }

            var valueType = rule.Value.GetType();

            if (valueType.IsArrayOrListType())
            {
                var arrayType = typeof(IEnumerable<>).MakeGenericType(conversionType);
                return Expression.Constant(rule.Value, arrayType);
            }
            else
            {
                var value = CastTo(rule.Value, conversionType);
                return Expression.Constant(value, conversionType);
            }
        }
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


    private static bool CheckFilterRule(Type type, DataAccessFilterRule rule)
    {
        if (rule.Value == null || rule.Value.ToString() == string.Empty)
        {
            rule.Value = null;
        }

        return rule.Value switch {
            null when (type == typeof(string) || type.IsNullableType()) =>
                rule.Operate == DataAccessFilterOperate.Equal || rule.Operate == DataAccessFilterOperate.NotEqual,
            null => !type.IsValueType,
            _ => true
        };
    }
    
    private static object CastTo(object value, Type conversionType)
    {
        if (conversionType == typeof(Guid) || conversionType == typeof(Guid?))
        {
            return TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(value.ToString()!)!;
        }
        return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
    }
}