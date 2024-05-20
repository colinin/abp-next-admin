using System;
using System.Linq.Expressions;

namespace LINGYUN.Abp.DataProtection.Operations;
public class DataAccessEndsWithContributor : IDataAccessOperateContributor
{
    public DataAccessFilterOperate Operate => DataAccessFilterOperate.EndsWith;

    public Expression BuildExpression(Expression left, Expression right)
    {
        if (left.Type != typeof(string))
        {
            throw new NotSupportedException("\"EndsWith\" only supports data of string type!");
        }
        return Expression.Call(left,
            typeof(string).GetMethod("EndsWith", new[] { typeof(string) })
            ?? throw new InvalidOperationException("The method named \"EndsWith\" does not exist!"),
            right);
    }
}
