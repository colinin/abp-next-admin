using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.Menus
{
    public class EfCoreMenuRepository : EfCoreRepository<PlatformDbContext, Menu, Guid>, IMenuRepository
    {
        public EfCoreMenuRepository(
            IDbContextProvider<PlatformDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> UserHasInMenuAsync(
            Guid userId,
            string menuName,
            CancellationToken cancellationToken = default)
        {
            var menuQuery = DbSet.Where(x => x.Name == menuName);

            return await (from menu in menuQuery
                          join userMenu in DbContext.Set<UserMenu>()
                               on menu.Id equals userMenu.MenuId
                          select userMenu)
                          .AnyAsync(x => x.UserId == userId, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> RoleHasInMenuAsync(
            string roleName,
            string menuName,
            CancellationToken cancellationToken = default)
        {
            var menuQuery = DbSet.Where(x => x.Name == menuName);

            return await (from menu in menuQuery
                          join roleMenu in DbContext.Set<RoleMenu>()
                               on menu.Id equals roleMenu.MenuId
                          select roleMenu)
                          .AnyAsync(x => x.RoleName == roleName, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Menu> FindByNameAsync(
            string menuName,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(x => x.Name == menuName)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Menu> FindMainAsync(
            PlatformType platformType = PlatformType.None,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(menu => menu.PlatformType.HasFlag(platformType) && menu.Path == "/")
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Menu>> GetRoleMenusAsync(
            string[] roles, 
            PlatformType platformType = PlatformType.None, 
            CancellationToken cancellationToken = default)
        {
            var menuQuery = DbSet
                .Where(menu => menu.PlatformType.HasFlag(platformType));

            var roleMenuQuery = DbContext.Set<RoleMenu>()
                    .Where(menu => roles.Contains(menu.RoleName));

            return await (from menu in menuQuery
                          join roleMenu in roleMenuQuery
                              on menu.Id equals roleMenu.MenuId
                          select menu)
                    .Union(menuQuery.Where(x => x.IsPublic))
                    .Distinct()
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Menu>> GetUserMenusAsync(
            Guid userId, 
            string[] roles, 
            PlatformType platformType = PlatformType.None, 
            CancellationToken cancellationToken = default)
        {
            var menuQuery = DbSet
                .Where(menu => menu.PlatformType.HasFlag(platformType));

            var userMenuQuery = from userMenu in DbContext.Set<UserMenu>()
                                join menu in menuQuery
                                    on userMenu.MenuId equals menu.Id
                                where userMenu.UserId == userId
                                select menu;

            if (roles != null && roles.Length > 0)
            {
                var roleMenuQuery = from roleMenu in DbContext.Set<RoleMenu>()
                                    join menu in menuQuery
                                        on roleMenu.MenuId equals menu.Id
                                    where roles.Contains(roleMenu.RoleName)
                                    select menu; ;

                return await userMenuQuery
                    .Union(roleMenuQuery)
                    .Union(menuQuery.Where(x => x.IsPublic))
                    .Distinct()
                    .ToListAsync(GetCancellationToken(cancellationToken));
            }

            return await userMenuQuery
                    .Union(menuQuery.Where(x => x.IsPublic))
                    .Distinct()
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Menu>> GetChildrenAsync(
            Guid? parentId,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet
                .Where(x => x.ParentId == parentId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Menu>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet
               .Where(x => x.Code.StartsWith(code) && x.Id != parentId.Value)
               .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Menu>> GetAllAsync(
            string filter = "",
            string sorting = nameof(Menu.Code),
            bool reverse = false,
            PlatformType? platformType = null,
            Guid? parentId = null,
            Guid? layoutId = null,
            CancellationToken cancellationToken = default)
        {
            sorting ??= nameof(Menu.Code);
            sorting = reverse ? sorting + " DESC" : sorting;

            return await DbSet
                .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
                .WhereIf(layoutId.HasValue, x => x.LayoutId == layoutId)
                .WhereIf(platformType.HasValue, menu => menu.PlatformType.HasFlag(platformType.Value))
                .WhereIf(!filter.IsNullOrWhiteSpace(), menu =>
                        menu.Path.Contains(filter) || menu.Name.Contains(filter) ||
                        menu.DisplayName.Contains(filter) || menu.Description.Contains(filter) ||
                        menu.Redirect.Contains(filter))
                .OrderBy(sorting)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetCountAsync(
            string filter = "",
            PlatformType? platformType = null,
            Guid? parentId = null,
            Guid? layoutId = null,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
                .WhereIf(layoutId.HasValue, x => x.LayoutId == layoutId)
                .WhereIf(platformType.HasValue, menu => menu.PlatformType.HasFlag(platformType.Value))
                .WhereIf(!filter.IsNullOrWhiteSpace(), menu => 
                        menu.Path.Contains(filter) || menu.Name.Contains(filter) ||
                        menu.DisplayName.Contains(filter) || menu.Description.Contains(filter) ||
                        menu.Redirect.Contains(filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Menu>> GetListAsync(
            string filter = "",
            string sorting = nameof(Menu.Code),
            bool reverse = false,
            PlatformType? platformType = null,
            Guid? parentId = null,
            Guid? layoutId = null,
            int skipCount = 0, 
            int maxResultCount = 10, 
            CancellationToken cancellationToken = default)
        {
            sorting ??= nameof(Menu.Code);
            sorting = reverse ? sorting + " DESC" : sorting;

            return await DbSet
                .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
                .WhereIf(layoutId.HasValue, x => x.LayoutId == layoutId)
                .WhereIf(platformType.HasValue, menu => menu.PlatformType.HasFlag(platformType.Value))
                .WhereIf(!filter.IsNullOrWhiteSpace(), menu =>
                        menu.Path.Contains(filter) || menu.Name.Contains(filter) ||
                        menu.DisplayName.Contains(filter) || menu.Description.Contains(filter) ||
                        menu.Redirect.Contains(filter))
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task RemoveAllRolesAsync(
            Menu menu,
            CancellationToken cancellationToken = default
        )
        {
            var rolesQuery = await DbContext.Set<RoleMenu>()
                .Where(q => q.MenuId == menu.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            DbContext.Set<RoleMenu>().RemoveRange(rolesQuery);
        }

        public virtual async Task RemoveAllMembersAsync(
            Menu menu,
            CancellationToken cancellationToken = default
        )
        {
            var membersQuery = await DbContext.Set<UserMenu>()
                .Where(q => q.MenuId == menu.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            DbContext.Set<UserMenu>().RemoveRange(membersQuery);
        }

        public override IQueryable<Menu> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
