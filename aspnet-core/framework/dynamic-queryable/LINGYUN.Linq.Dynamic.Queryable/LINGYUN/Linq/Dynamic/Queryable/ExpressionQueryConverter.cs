using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LINGYUN.Linq.Dynamic.Queryable;

public class ExpressionQueryConverter<TSource, TTarget> : ExpressionVisitor
{
    private readonly IReadOnlyDictionary<Type, Type> _typeMap;
    private readonly Dictionary<ParameterExpression, ParameterExpression> _parameterMap = new();

    public ExpressionQueryConverter(IReadOnlyDictionary<Type, Type> typeMap)
    {
        _typeMap = typeMap ?? throw new ArgumentNullException(nameof(typeMap));
    }

    public Expression<Func<TTarget, bool>> Convert(Expression<Func<TSource, bool>> expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        _parameterMap.Clear();
        var rootParameter = Expression.Parameter(typeof(TTarget), expression.Parameters[0].Name);
        _parameterMap[expression.Parameters[0]] = rootParameter;

        var body = Visit(expression.Body);
        return Expression.Lambda<Func<TTarget, bool>>(body, rootParameter);
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        var parameters = node.Parameters.Select(p =>
        {
            if (_parameterMap.TryGetValue(p, out var mapped))
            {
                return mapped;
            }
            if (_typeMap.TryGetValue(p.Type, out var targetType))
            {
                mapped = Expression.Parameter(targetType, p.Name);
                _parameterMap[p] = mapped;
                return mapped;
            }
            return p;
        }).ToArray();

        var body = Visit(node.Body);
        return Expression.Lambda(body, parameters);
    }

    protected override Expression VisitParameter(ParameterExpression node)
        => _parameterMap.TryGetValue(node, out var mapped) ? mapped : node;

    protected override Expression VisitMember(MemberExpression node)
    {
        var expression = Visit(node.Expression);

        if (expression != null
            && node.Member is PropertyInfo property
            && _typeMap.ContainsKey(property.DeclaringType!))
        {
            var targetProperty = expression.Type.GetProperty(
                property.Name,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            if (targetProperty == null)
            {
                throw new NotSupportedException(
                    $"The property {property.Name} could not be found on the target type {expression.Type.FullName} and thus the expression cannot be overridden.");
            }
            return Expression.MakeMemberAccess(expression, targetProperty);
        }

        return node.Expression == expression
            ? node
            : Expression.MakeMemberAccess(expression, node.Member);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.IsGenericMethod)
        {
            var oldArguments = node.Method.GetGenericArguments();
            var newArguments = oldArguments
                .Select(a => _typeMap.TryGetValue(a, out var mapped) ? mapped : a)
                .ToArray();

            if (!oldArguments.SequenceEqual(newArguments))
            {
                var targetMethod = node.Method.GetGenericMethodDefinition().MakeGenericMethod(newArguments);
                var instance = node.Object != null ? Visit(node.Object) : null;
                var arguments = node.Arguments.Select(Visit).ToArray();
                return Expression.Call(instance, targetMethod, arguments!);
            }
        }

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (node.NodeType == ExpressionType.Quote)
        {
            var operand = Visit(node.Operand);
            return Expression.Quote(operand);
        }

        if (node.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked)
        {
            var operand = Visit(node.Operand);
            var resultType = _typeMap.TryGetValue(node.Type, out var mappedType) ? mappedType : node.Type;

            if (operand.Type == resultType)
            {
                return operand;
            }
            return Expression.Convert(operand, resultType);
        }

        return base.VisitUnary(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Value is Enum enumValue && _typeMap.TryGetValue(enumValue.GetType(), out var targetType))
        {
            return Expression.Constant(Enum.ToObject(targetType, System.Convert.ToInt64(enumValue)), targetType);
        }
        return base.VisitConstant(node);
    }
}
