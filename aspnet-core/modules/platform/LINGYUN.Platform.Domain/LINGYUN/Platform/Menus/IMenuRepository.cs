using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Menus
{
    public interface IMenuRepository : IBasicRepository<Menu, Guid>
    {
        /// <summary>
        /// 获取最后一个菜单
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Menu> GetLastMenuAsync(
            Guid? parentId = null,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据名称查询菜单
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Menu> FindByNameAsync(
            string menuName,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询主菜单,每一个布局页创建的时候都要创建路径为 / 的主菜单
        /// </summary>
        /// <param name="platformType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Menu> FindMainAsync(
            string framework = "",
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Menu>> GetChildrenAsync(
            Guid? parentId,
            CancellationToken cancellationToken = default
        );
        /// <summary>
        /// 通过父菜单编码查询子菜单
        /// </summary>
        /// <param name="code"></param>
        /// <param name="parentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Menu>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            CancellationToken cancellationToken = default
        );
        /// <summary>
        /// 查找用户可访问菜单
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <param name="roles">角色列表</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Menu>> GetUserMenusAsync(
            Guid userId,
            string[] roles,
            string framework = "",
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 查找角色可访问菜单
        /// </summary>
        /// <param name="roles">角色列表</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Menu>> GetRoleMenusAsync(
            string[] roles,
            string framework = "",
            CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(
            string filter = "",
            string framework = "",
            Guid? parentId = null,
            Guid? layoutId = null,
            CancellationToken cancellationToken = default);

        Task<List<Menu>> GetListAsync(
            string filter = "",
            string sorting = nameof(Menu.Code),
            bool reverse = false,
            string framework = "",
            Guid? parentId = null,
            Guid? layoutId = null,
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<List<Menu>> GetAllAsync(
            string filter = "",
            string sorting = nameof(Menu.Code),
            bool reverse = false,
            string framework = "",
            Guid? parentId = null,
            Guid? layoutId = null,
            CancellationToken cancellationToken = default);

        Task RemoveAllRolesAsync(
            Menu menu,
            CancellationToken cancellationToken = default
        );

        Task RemoveAllMembersAsync(
            Menu menu,
            CancellationToken cancellationToken = default
        );
    }
}
