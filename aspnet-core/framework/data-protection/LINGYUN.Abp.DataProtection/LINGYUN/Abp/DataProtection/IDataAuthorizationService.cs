using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据权限验证服务
/// </summary>
public interface IDataAuthorizationService
{
    /// <summary>
    /// 验证操作实体数据权限
    /// </summary>
    /// <param name="operation">数据权限操作</param>
    /// <param name="entities">检查实体列表</param>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <returns></returns>
    Task<AuthorizationResult> AuthorizeAsync<TEntity>(DataAccessOperation operation, IEnumerable<TEntity> entities);
}