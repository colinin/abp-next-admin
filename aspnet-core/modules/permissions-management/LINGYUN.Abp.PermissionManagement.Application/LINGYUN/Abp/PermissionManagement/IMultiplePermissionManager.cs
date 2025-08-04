using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement;
public interface IMultiplePermissionManager : IPermissionManager
{
    /// <summary>
    /// 批量设置权限
    /// </summary>
    /// <param name="providerName">权限提供者</param>
    /// <param name="providerKey">权限提供者Key</param>
    /// <param name="permissions">权限集合</param>
    /// <returns></returns>
    Task SetManyAsync(string providerName, string providerKey, IEnumerable<PermissionChangeState> permissions);
}
