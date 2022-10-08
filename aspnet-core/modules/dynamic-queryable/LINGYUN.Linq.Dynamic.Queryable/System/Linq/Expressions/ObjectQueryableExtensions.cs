using LINGYUN.Linq.Dynamic.Queryable;
using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions;

public static class ObjectQueryableExtensions
{
    public static Expression<T> DynamicQuery<T>(
        this Expression<T> condition,
        DynamicQueryable queryable)
    {
        var typeExpression = condition.Parameters.FirstOrDefault();

        return BuildExpressions(condition, typeExpression, queryable.Paramters);
    }

    private static Expression<T> BuildExpressions<T>(
        Expression<T> condition,
        Expression typeExpression,
        List<DynamicParamter> paramters)
    {
        var expressions = new Stack<Expression>();
        foreach (var paramter in paramters)
        {
            Expression exp = null;
            Type propertyType = null;
            var leftParamter = Expression.PropertyOrField(typeExpression, paramter.Filed);
            if (!string.IsNullOrWhiteSpace(paramter.Type))
            {
                propertyType = Type.GetType(paramter.Type, true);
            }
            else
            {
                propertyType = (leftParamter.Member as PropertyInfo)?.PropertyType ?? paramter.Value.GetType();
            }
            var rightParamter = Expression.Convert(Expression.Constant(paramter.Value), propertyType);
            switch (paramter.Comparison)
            {
                case DynamicComparison.Equal:
                    exp = Expression.Equal(leftParamter, rightParamter);
                    break;
                case DynamicComparison.NotEqual:
                    exp = Expression.NotEqual(leftParamter, rightParamter);
                    break;
                case DynamicComparison.LessThan:
                    exp = Expression.LessThan(leftParamter, rightParamter);
                    break;
                case DynamicComparison.LessThanOrEqual:
                    exp = Expression.LessThanOrEqual(leftParamter, rightParamter);
                    break;
                case DynamicComparison.GreaterThan:
                    exp = Expression.GreaterThan(leftParamter, rightParamter);
                    break;
                case DynamicComparison.GreaterThanOrEqual:
                    exp = Expression.GreaterThanOrEqual(leftParamter, rightParamter);
                    break;
                case DynamicComparison.StartsWith:
                    exp = Expression.Call(
                        leftParamter,
                        typeof(string).GetMethod(nameof(String.StartsWith), new[] { typeof(string) }),
                        rightParamter);
                    break;
                case DynamicComparison.NotStartsWith:
                    exp = Expression.Not(
                        Expression.Call(
                            leftParamter,
                            typeof(string).GetMethod(nameof(String.StartsWith), new[] { typeof(string) }),
                            rightParamter));
                    break;
                case DynamicComparison.EndsWith:
                    exp = Expression.Call(
                        leftParamter,
                        typeof(string).GetMethod(nameof(String.EndsWith), new[] { typeof(string) }),
                        rightParamter);
                    break;
                case DynamicComparison.NotEndsWith:
                    exp = Expression.Not(
                        Expression.Call(
                            leftParamter,
                            typeof(string).GetMethod(nameof(String.EndsWith), new[] { typeof(string) }),
                            rightParamter));
                    break;
                case DynamicComparison.Contains:
                    exp = Expression.Call(
                        leftParamter,
                        typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) }),
                        rightParamter);
                    break;
                case DynamicComparison.NotContains:
                    exp = Expression.Not(
                        Expression.Call(
                            leftParamter,
                            typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) }),
                            rightParamter));
                    break;
            }
            expressions.Push(exp);

            while (expressions.Count > 1)
            {
                var exp1 = expressions.Pop();
                var exp2 = expressions.Pop();

                switch (paramter.Logic)
                {
                    case DynamicLogic.And:
                        expressions.Push(Expression.AndAlso(exp1, exp2));
                        break;
                    case DynamicLogic.Or:
                        expressions.Push(Expression.Or(exp1, exp2));
                        break;
                }
            }
        }

        return Expression.Lambda<T>(expressions.Pop(), condition.Parameters.ToArray());
    }
}
