namespace System.Linq.Expressions;

internal static class ExpressionFuncExtensions
{
    public static Expression<Func<T, bool>> AndIf<T>(
        this Expression<Func<T, bool>> first,
        bool condition,
        Expression<Func<T, bool>> second)
    {
        if (condition)
        {
            return first.And(second);
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
            return first.Or(second);
        }

        return first;
    }
}
