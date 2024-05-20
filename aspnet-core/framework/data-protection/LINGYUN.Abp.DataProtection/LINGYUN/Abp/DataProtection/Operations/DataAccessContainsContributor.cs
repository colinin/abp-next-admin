using System;
using System.Linq;
using System.Linq.Expressions;

namespace LINGYUN.Abp.DataProtection.Operations;
public class DataAccessContainsContributor : IDataAccessOperateContributor
{
    public DataAccessFilterOperate Operate => DataAccessFilterOperate.Contains;

    public Expression BuildExpression(Expression left, Expression right)
    {
        if (left.Type == typeof(string))
        {
            return Expression.Call(left,
                typeof(string).GetMethod("Contains", new[] { typeof(string) })
                ?? throw new InvalidOperationException("The method named \"Contains\" does not exist"),
                right);
        }

        if (left.Type.IsArrayOrListType())
        {
            var method = typeof(Enumerable).GetMethods().Where(x => x.Name == "Contains").FirstOrDefault();
            var methodType = method?.MakeGenericMethod(left.Type.GetGenericArguments()[0]);
            return Expression.Call(
                null,
                methodType ?? throw new InvalidOperationException("The method named \"Contains\" does not exist"),
                left,
                right);
        }

        throw new NotSupportedException("\"Contains\" only supports data of string or array type!");
    }
}
