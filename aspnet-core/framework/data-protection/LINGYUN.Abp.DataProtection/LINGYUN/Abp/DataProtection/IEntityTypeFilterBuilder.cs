using System;
using System.Linq.Expressions;

namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 实体过滤条件构造器
/// </summary>
public interface IEntityTypeFilterBuilder
{
    /// <summary>
    /// 获取实体的查询过滤表达式
    /// </summary>
    /// <param name="operation">数据权限操作</param>
    /// <param name="group">查询条件组</param>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <returns></returns>
    Expression<Func<TEntity, bool>> Build<TEntity>(DataAccessOperation operation, DataAccessFilterGroup group = null);

    LambdaExpression Build(Type entityType, DataAccessOperation operation, DataAccessFilterGroup group = null);
}