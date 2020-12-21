using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Menus
{
    public interface IRoleMenuRepository : IBasicRepository<RoleMenu, Guid>
    {
        /// <summary>
        /// 角色是否拥有菜单
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="menuName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> RoleHasInMenuAsync(
            string roleName,
            string menuName,
            CancellationToken cancellationToken = default);

        Task SetRoleMenusAsync(
            string roleName,
            IEnumerable<Guid> menuIds,
            CancellationToken cancellationToken = default);
    }
}
