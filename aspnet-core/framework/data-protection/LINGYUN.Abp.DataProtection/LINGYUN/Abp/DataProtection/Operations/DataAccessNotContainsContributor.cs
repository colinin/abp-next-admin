using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LINGYUN.Abp.DataProtection.Operations;
public class DataAccessNotContainsContributor : IDataAccessOperateContributor
{
    public DataAccessFilterOperate Operate => DataAccessFilterOperate.NotContains;

    public Expression BuildExpression(Expression left, Expression right)
    {

        if (left.Type == typeof(string))
        {
            // string.Contains(string);
            return Expression.Not(Expression.Call(left,
                typeof(string).GetMethod("Contains", new[] { typeof(string) })
                ?? throw new InvalidOperationException("The method named \"NotContains\" does not exist"),
                right));
        }

        if (left.Type.IsArrayOrListType())
        {
            // IEnumerable<T>.Contains(T);
            var method = typeof(Enumerable).GetMethods().Where(x => x.Name == "Contains").FirstOrDefault();
            var methodType = method?.MakeGenericMethod(left.Type.GetGenericArguments()[0]);
            return Expression.Not(Expression.Call(
                null,
                methodType ?? throw new InvalidOperationException("The method named \"NotContains\" does not exist"),
                left,
                right));
        }

        throw new NotSupportedException("\"NotContains\" only supports data of string or array type!");
    }
}
