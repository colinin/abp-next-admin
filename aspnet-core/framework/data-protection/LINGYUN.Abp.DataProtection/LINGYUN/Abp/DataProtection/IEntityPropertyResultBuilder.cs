using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 实体属性返回值构造器
/// </summary>
public interface IEntityPropertyResultBuilder
{
    /// <summary>
    /// 获取实体的属性返回值过滤表达式
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="operation">数据操作方式</param>
    /// <returns></returns>
    Task<Expression<Func<TEntity, TEntity>>> Build<TEntity>(DataAccessOperation operation);
    /// <summary>
    /// 获取实体的属性返回值过滤表达式
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <param name="operation">数据操作方式</param>
    /// <returns></returns>
    Task<LambdaExpression> Build(Type entityType, DataAccessOperation operation);
}
