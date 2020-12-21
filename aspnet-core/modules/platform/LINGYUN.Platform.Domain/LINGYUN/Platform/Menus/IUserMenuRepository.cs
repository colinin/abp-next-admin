using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Menus
{
    public interface IUserMenuRepository : IBasicRepository<UserMenu, Guid>
    {
        /// <summary>
        /// 用户是否拥有菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UserHasInMenuAsync(
            Guid userId,
            string menuName,
            CancellationToken cancellationToken = default);

        Task SetMemberMenusAsync(
            Guid userId,
            IEnumerable<Guid> menuIds,
            CancellationToken cancellationToken = default);
    }
}
