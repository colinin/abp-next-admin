using Volo.Abp.Specifications;

namespace System.Linq.Expressions;

public static class ExpressionFuncExtensions
{
    public static Expression<Func<T, bool>> AndIf<T>(
        this Expression<Func<T, bool>> first,
        bool condition,
        Expression<Func<T, bool>> second)
    {
        if (condition)
        {
            return ExpressionFuncExtender.And(first, second);
        }

        return first;
    }

    public static Expression<Func<T, bool>> OrIf<T>(
        this Expression<Func<T, bool>> first,
        bool condition,
        Expression<Func<T, bool>> second)
    {
        if (condition)
        {
            return ExpressionFuncExtender.Or(first, second);
        }

        return first;
    }
}
