using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Utils;

public static class ExpressionParser
{
    public static List<Condition> ExtractFields<T>(Expression<Func<T, bool>> expression)
    {
        var result = new List<Condition>();
        ParseNode(expression.Body, result);
        return result;
    }

    private static void ParseNode(Expression node, List<Condition> result)
    {
        switch (node)
        {
            case BinaryExpression binary when binary.NodeType == ExpressionType.AndAlso
                                          || binary.NodeType == ExpressionType.OrElse:
                ParseNode(binary.Left, result);
                ParseNode(binary.Right, result);
                break;

            case BinaryExpression binary when TryMapOp(binary.NodeType, out var op):
                AddCondition(binary.Left, binary.Right, op, result);
                break;

            case MethodCallExpression methodCall:
                ParseMethodCall(methodCall, result);
                break;

            case UnaryExpression unary when unary.NodeType == ExpressionType.Not:
                ParseNode(unary.Operand, result);
                break;
        }
    }

    private static bool TryMapOp(ExpressionType nodeType, out ComparisonOp op)
    {
        op = nodeType switch
        {
            ExpressionType.Equal => ComparisonOp.Equal,
            ExpressionType.NotEqual => ComparisonOp.NotEqual,
            ExpressionType.GreaterThan => ComparisonOp.GreaterThan,
            ExpressionType.GreaterThanOrEqual => ComparisonOp.GreaterThanOrEqual,
            ExpressionType.LessThan => ComparisonOp.LessThan,
            ExpressionType.LessThanOrEqual => ComparisonOp.LessThanOrEqual,
            _ => default
        };
        return nodeType is ExpressionType.Equal
                       or ExpressionType.NotEqual
                       or ExpressionType.GreaterThan
                       or ExpressionType.GreaterThanOrEqual
                       or ExpressionType.LessThan
                       or ExpressionType.LessThanOrEqual;
    }

    private static void AddCondition(Expression left, Expression right, ComparisonOp op, List<Condition> result)
    {
        var (fieldExpr, valueExpr) = IsMemberAccess(left)
            ? (left, right)
            : (right, left);

        var fieldName = GetMemberPath(fieldExpr);
        if (fieldName == null)
        {
            return;
        }

        if (valueExpr is ConstantExpression { Value: null })
        {
            var nullOp = op switch
            {
                ComparisonOp.Equal => ComparisonOp.IsNull,
                ComparisonOp.NotEqual => ComparisonOp.IsNotNull,
                _ => throw new NotSupportedException("null only supports == or !=")
            };
            result.Add(new Condition(fieldName, nullOp, null));
            return;
        }

        var value = GetValue(valueExpr);
        result.Add(new Condition(fieldName, op, value));
    }

    private static void ParseMethodCall(MethodCallExpression methodCall, List<Condition> result)
    {
        if (methodCall.Object != null && IsMemberAccess(methodCall.Object))
        {
            var fieldName = GetMemberPath(methodCall.Object);
            var methodName = methodCall.Method.Name;

            if (methodCall.Method.Name == "Any" && methodCall.Arguments.Count == 2)
            {
                var collectionPath = GetMemberPath(methodCall.Arguments[0]);
                var predicate = (LambdaExpression)StripQuote(methodCall.Arguments[1]);
                var innerConditions = new List<Condition>();
                ParseNode(predicate.Body, innerConditions);

                foreach (var inner in innerConditions)
                {
                    var fullField = string.IsNullOrEmpty(collectionPath)
                        ? inner.Field
                        : $"{collectionPath}.{inner.Field}";
                    result.Add(new Condition(fullField, inner.Op, inner.Value));
                }
                return;
            }

            if (methodCall.Arguments.Count == 1)
            {
                var value = GetValue(methodCall.Arguments[0]);
                var op = methodName switch
                {
                    "Contains" => ComparisonOp.Contains,
                    "StartsWith" => ComparisonOp.StartsWith,
                    "EndsWith" => ComparisonOp.EndsWith,
                    _ => (ComparisonOp?)null
                };

                if (op.HasValue && fieldName != null)
                {
                    result.Add(new Condition(fieldName, op.Value, value));
                }
            }
        }
        else if (methodCall.Method.Name == "Contains" && methodCall.Arguments.Count == 2)
        {
            var fieldName = GetMemberPath(methodCall.Arguments[0]);
            if (fieldName != null)
            {
                var value = GetValue(methodCall.Arguments[1]);
                result.Add(new Condition(fieldName, ComparisonOp.Contains, value));
            }
        }
    }

    private static bool IsMemberAccess(Expression expr)
    {
        return expr is MemberExpression || expr is UnaryExpression { Operand: MemberExpression };
    }

    private static string? GetMemberPath(Expression expr)
    {
        if (expr is UnaryExpression { NodeType: ExpressionType.Convert } unary)
        {
            expr = unary.Operand;
        }

        if (expr is not MemberExpression member)
        {
            return null;
        }

        if (member.Expression is MemberExpression)
        {
            var parent = GetMemberPath(member.Expression);
            return parent == null ? member.Member.Name : $"{parent}.{member.Member.Name}";
        }

        return member.Member.Name;
    }

    private static object? GetValue(Expression expr)
    {
        if (expr is UnaryExpression { NodeType: ExpressionType.Convert } unary)
        {
            expr = unary.Operand;
        }

        if (expr is ConstantExpression constant)
        {
            return constant.Value;
        }

        if (expr is MemberExpression)
        {
            var objectMember = Expression.Convert(expr, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        var lambda = Expression.Lambda(expr);
        return lambda.Compile().DynamicInvoke();
    }

    private static Expression StripQuote(Expression expr)
    {
        return expr is UnaryExpression { NodeType: ExpressionType.Quote } unary ? unary.Operand : expr;
    }
}