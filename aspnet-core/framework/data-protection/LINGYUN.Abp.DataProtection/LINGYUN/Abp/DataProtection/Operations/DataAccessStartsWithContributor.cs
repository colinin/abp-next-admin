using System;
using System.Linq.Expressions;

namespace LINGYUN.Abp.DataProtection.Operations;
public class DataAccessStartsWithContributor : IDataAccessOperateContributor
{
    public DataAccessFilterOperate Operate => DataAccessFilterOperate.StartsWith;

    public Expression BuildExpression(Expression left, Expression right)
    {
        if (left.Type != typeof(string))
        {
            throw new NotSupportedException("\"StartsWith\" only supports data of string type!");
        }
        return Expression.Call(left,
            typeof(string).GetMethod("StartsWith", new[] { typeof(string) })
            ?? throw new InvalidOperationException("The method named \"StartsWith\" does not exist!"),
            right);
    }
}
