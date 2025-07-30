using LINGYUN.Linq.Dynamic.Queryable;
using LINGYUN.Linq.Dynamic.Queryable.Reflection;
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
            var leftParamter = Expression.PropertyOrField(typeExpression, paramter.Field);
            if (!string.IsNullOrWhiteSpace(paramter.Type))
            {
                propertyType = Type.GetType(paramter.Type, true);
            }
            else
            {
                propertyType = (leftParamter.Member as PropertyInfo)?.PropertyType ?? paramter.Value.GetType();
            }
            switch (paramter.Comparison)
            {
                case DynamicComparison.NotEqual:
                    // For example(MySql): 
                    // ...Other (Field <> Value)
                    exp = Expression.NotEqual(
                        leftParamter,
                        GetValue(paramter, propertyType));
                    break;
                case DynamicComparison.LessThan:
                    // For example(MySql): 
                    // ...Other (Field < Value)
                    exp = BuildLessThanExpression(paramter, leftParamter, propertyType);
                    break;
                case DynamicComparison.LessThanOrEqual:
                    // For example(MySql): 
                    // ...Other (Field <= Value)

                    exp = BuildLessThanOrEqualExpression(paramter, leftParamter, propertyType);
                    break;
                case DynamicComparison.GreaterThan:
                    // For example(MySql): 
                    // ...Other (Field > Value)
                    exp = BuildGreaterThanExpression(paramter, leftParamter, propertyType);
                    break;
                case DynamicComparison.GreaterThanOrEqual:
                    // For example(MySql): 
                    // ...Other (Field >= Value)

                    exp = BuildGreaterThanOrEqualExpression(paramter, leftParamter, propertyType);
                    break;
                case DynamicComparison.StartsWith:
                    // For example(MySql): 
                    // ...Other And Field LIKE 'Value%'
                    exp = Expression.Call(
                        leftParamter,
                        typeof(string).GetMethod(nameof(String.StartsWith), new[] { typeof(string) }),
                        GetValue(paramter, propertyType));

                    // TODO: 单元测试通过
                    // For example(MySql): 
                    // ...Other ((Field IS NOT NULL) AND (Field LIKE 'Value%'))
                    //var startsWithNotNullExp = Expression.Not(
                    //    Expression.Equal(leftParamter,
                    //        Expression.Convert(
                    //            Expression.Constant(null), propertyType)));

                    //var startsWithExp = Expression.Call(
                    //    leftParamter,
                    //    typeof(string).GetMethod(nameof(String.StartsWith), new[] { typeof(string) }),
                    //    rightParamter);

                    //exp = Expression.AndAlso(startsWithNotNullExp, startsWithExp);

                    break;
                case DynamicComparison.NotStartsWith:
                    // For example(MySql): 
                    // ...Other NOT Field LIKE 'Value%'

                    exp = Expression.Not(
                        Expression.Call(
                            leftParamter,
                            typeof(string).GetMethod(nameof(String.StartsWith), new[] { typeof(string) }),
                            GetValue(paramter, propertyType)));

                    // TODO: 单元测试通过
                    // For example(MySql): 
                    // ...Other ((Field IS NULL) OR NOT (Field LIKE 'Value%'))
                    //var notStartsWithNullExp = Expression.Equal(leftParamter,
                    //        Expression.Convert(
                    //            Expression.Constant(null), propertyType));

                    //var notStartsWithExp = Expression.Not(
                    //    Expression.Call(
                    //        leftParamter,
                    //        typeof(string).GetMethod(nameof(String.StartsWith), new[] { typeof(string) }),
                    //        rightParamter));

                    //exp = Expression.OrElse(notStartsWithNullExp, notStartsWithExp);
                    break;
                case DynamicComparison.EndsWith:
                    // For example(MySql): 
                    // ...Other AND Field LIKE '%Value'
                    exp = Expression.Call(
                        leftParamter,
                        typeof(string).GetMethod(nameof(String.EndsWith), new[] { typeof(string) }),
                        GetValue(paramter, propertyType));

                    // TODO: 单元测试通过
                    // For example(MySql): 
                    // ...Other ((Field IS NOT NULL) AND (Field LIKE '%Value'))
                    //var endsWithNotNullExp = Expression.Not(
                    //    Expression.Equal(leftParamter,
                    //        Expression.Convert(
                    //            Expression.Constant(null), propertyType)));

                    //var endsWithExp = Expression.Call(
                    //    leftParamter,
                    //    typeof(string).GetMethod(nameof(String.EndsWith), new[] { typeof(string) }),
                    //    rightParamter);

                    //exp = Expression.AndAlso(endsWithNotNullExp, endsWithExp);
                    break;
                case DynamicComparison.NotEndsWith:
                    // For example(MySql): 
                    // ...Other NOT (Field LIKE '%Value')
                    exp = Expression.Not(
                        Expression.Call(
                            leftParamter,
                            typeof(string).GetMethod(nameof(String.EndsWith), new[] { typeof(string) }),
                            GetValue(paramter, propertyType)));

                    // TODO: 单元测试通过
                    // For example(MySql): 
                    // ...Other ((Field IS NULL) OR NOT (Field LIKE '%Value'))
                    //var notEndsWithNullExp = Expression.Equal(leftParamter,
                    //        Expression.Convert(
                    //            Expression.Constant(null), propertyType));

                    //var notEndsWithExp = Expression.Not(
                    //    Expression.Call(
                    //        leftParamter,
                    //        typeof(string).GetMethod(nameof(String.EndsWith), new[] { typeof(string) }),
                    //        rightParamter));

                    //exp = Expression.OrElse(notEndsWithNullExp, notEndsWithExp);
                    break;
                case DynamicComparison.Contains:
                    // For example(MySql): 
                    // ...Other AND (Field LIKE '%Value%')
                    exp = Expression.Call(
                        leftParamter,
                        typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) }),
                        GetValue(paramter, propertyType));

                    // TODO: 单元测试通过
                    // For example(MySql): 
                    // ...Other ((Field IS NOT NULL) AND (Field LIKE '%Value%'))
                    //var containsNotNullExp = Expression.Not(
                    //    Expression.Equal(leftParamter,
                    //        Expression.Convert(
                    //            Expression.Constant(null), propertyType)));

                    //var containsExp = Expression.Call(
                    //    leftParamter,
                    //    typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) }),
                    //    rightParamter);

                    //exp = Expression.AndAlso(containsNotNullExp, containsExp);
                    break;
                case DynamicComparison.NotContains:
                    // For example(MySql): 
                    // ...Other AND (Field NOT LIKE '%Value%')
                    exp = Expression.Not(
                       Expression.Call(
                           leftParamter,
                           typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) }),
                           GetValue(paramter, propertyType)));
                    // TODO: 单元测试通过
                    // For example(MySql): 
                    // ...Other ((Field IS NULL) OR (Field NOT LIKE '%Value%'))

                    //var notContainsNullExp = Expression.Equal(leftParamter,
                    //        Expression.Convert(
                    //            Expression.Constant(null), propertyType));

                    //var notContainsExp = Expression.Not(
                    //    Expression.Call(
                    //        leftParamter,
                    //        typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) }),
                    //        rightParamter));

                    //exp = Expression.OrElse(notContainsNullExp, notContainsExp);
                    break;
                case DynamicComparison.Null:
                    // For example(MySql): 
                    // ...Other (Field IS NULL)

                    // 非空字段设定为比对默认值
                    exp = Expression.Equal(leftParamter,
                        Expression.Constant(GetDefaultValue(propertyType)));
                    break;
                case DynamicComparison.NotNull:
                    // For example(MySql): 
                    // ...Other (Field IS NOT NULL)

                    exp = Expression.NotEqual(leftParamter,
                        Expression.Constant(GetDefaultValue(propertyType)));
                    break;
                default:
                case DynamicComparison.Equal:
                    // For example(MySql): 
                    // ...Other (Field = Value)

                    exp = Expression.Equal(
                        leftParamter,
                        GetValue(paramter, propertyType));
                    break;
            }
            expressions.Push(exp);

            while (expressions.Count > 1)
            {
                var exp1 = expressions.Pop();
                var exp2 = expressions.Pop();

                switch (paramter.Logic)
                {
                    case DynamicLogic.Or:
                        expressions.Push(Expression.Or(exp1, exp2));
                        break;
                    default:
                    case DynamicLogic.And:
                        expressions.Push(Expression.AndAlso(exp1, exp2));
                        break;
                }
            }
        }

        return Expression.Lambda<T>(expressions.Pop(), condition.Parameters.ToArray());
    }

    private static Expression BuildLessThanExpression(DynamicParamter paramter, MemberExpression member, Type propertyType)
    {
        if (propertyType == typeof(string))
        {
            // 字符串比较: Field < Value
            return Expression.LessThan(
                Expression.Call(
                    member,
                    typeof(string).GetMethod("CompareTo", new[] { typeof(string) }),
                    Expression.Constant(Convert.ToString(paramter.Value))),
                Expression.Constant(0));
        }
        if (propertyType.IsNullableType())
        {
            // 可空类型比较: Field < Value
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            var hasValue = Expression.Property(member, "HasValue");
            var value = Expression.Property(member, "Value");

            return Expression.AndAlso(
                hasValue,
                Expression.LessThan(
                    value,
                    GetValue(paramter, underlyingType)));
        }
        else
        {
            // 数值比较: Field < Value
            return Expression.LessThan(
                member,
                GetValue(paramter, propertyType));
        }
    }

    private static Expression BuildLessThanOrEqualExpression(DynamicParamter paramter, MemberExpression member, Type propertyType)
    {
        if (propertyType == typeof(string))
        {
            // 字符串比较: Field <= Value
            return Expression.LessThanOrEqual(
                Expression.Call(
                    member,
                    typeof(string).GetMethod("CompareTo", new[] { typeof(string) }),
                    Expression.Constant(Convert.ToString(paramter.Value))),
                Expression.Constant(0));
        }
        if (propertyType.IsNullableType())
        {
            // 可空类型比较: Field <= Value
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            var hasValue = Expression.Property(member, "HasValue");
            var value = Expression.Property(member, "Value");

            return Expression.AndAlso(
                hasValue,
                Expression.LessThanOrEqual(
                    value,
                    GetValue(paramter, underlyingType)));
        }
        else
        {
            // 数值比较: Field <= Value
            return Expression.LessThanOrEqual(
                member,
                GetValue(paramter, propertyType));
        }
    }

    private static Expression BuildGreaterThanExpression(DynamicParamter paramter, MemberExpression member, Type propertyType)
    {
        if (propertyType == typeof(string))
        {
            // 字符串比较: Field > Value
            return Expression.GreaterThan(
                Expression.Call(
                    member,
                    typeof(string).GetMethod("CompareTo", new[] { typeof(string) }),
                    Expression.Constant(Convert.ToString(paramter.Value))),
                Expression.Constant(0));
        }
        if (propertyType.IsNullableType())
        {
            // 可空类型比较: Field > Value
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            var hasValue = Expression.Property(member, "HasValue");
            var value = Expression.Property(member, "Value");

            return Expression.AndAlso(
                hasValue,
                Expression.GreaterThan(
                    value,
                    GetValue(paramter, underlyingType)));
        }
        else
        {
            // 数值比较: Field > Value
            return Expression.GreaterThan(
                member,
                GetValue(paramter, propertyType));
        }
    }

    private static Expression BuildGreaterThanOrEqualExpression(DynamicParamter paramter, MemberExpression member, Type propertyType)
    {
        if (propertyType == typeof(string))
        {
            // 字符串比较: Field >= Value
            return Expression.GreaterThanOrEqual(
                Expression.Call(
                    member,
                    typeof(string).GetMethod("CompareTo", new[] { typeof(string) }),
                    Expression.Constant(Convert.ToString(paramter.Value))),
                Expression.Constant(0));
        }

        if (propertyType.IsNullableType())
        {
            // 可空类型比较: Field >= Value
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            var hasValue = Expression.Property(member, "HasValue");
            var value = Expression.Property(member, "Value");

            return Expression.AndAlso(
                hasValue,
                Expression.GreaterThanOrEqual(
                    value,
                    GetValue(paramter, underlyingType)));
        }
        else
        {
            // 数值比较: Field >= Value
            return Expression.GreaterThanOrEqual(
                member,
                GetValue(paramter, propertyType));
        }
    }

    private static ConstantExpression GetValue(DynamicParamter paramter, Type propertyType)
    {
        object typedValue;
        if (propertyType.IsNullableType())
        {
            propertyType = Nullable.GetUnderlyingType(propertyType);
        }

        typedValue = Convert.ChangeType(paramter.Value, propertyType);

        return Expression.Constant(typedValue, propertyType);
    }

    private static object GetDefaultValue(Type type)
    {
        // TODO: 非空字段此处返回默认值
        if (type.IsNullableType())
        {
            return null;
        }
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
