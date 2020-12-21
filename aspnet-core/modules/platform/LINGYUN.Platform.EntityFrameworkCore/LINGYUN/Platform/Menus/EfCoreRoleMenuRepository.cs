using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.Menus
{
    public class EfCoreRoleMenuRepository : EfCoreRepository<PlatformDbContext, RoleMenu, Guid>, IRoleMenuRepository
    {
        public EfCoreRoleMenuRepository(
            IDbContextProvider<PlatformDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> RoleHasInMenuAsync(
            string roleName, 
            string menuName, 
            CancellationToken cancellationToken = default)
        {
            var menuQuery = DbContext.Set<Menu>().Where(x => x.Name == menuName);

            return await
                (from roleMenu in DbSet
                 join menu in menuQuery
                      on roleMenu.MenuId equals menu.Id
                 select roleMenu)
                 .AnyAsync(x => x.RoleName == roleName, 
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task SetRoleMenusAsync(
            string roleName,
            IEnumerable<Guid> menuIds,
            CancellationToken cancellationToken = default)
        {
            var hasInMenus = await DbSet
                .Where(x => x.RoleName == roleName)
                .ToArrayAsync(GetCancellationToken(cancellationToken));

            var removes = hasInMenus.Where(x => !menuIds.Contains(x.MenuId));
            if (removes.Any())
            {
                DbContext.RemoveRange(removes);
            }

            var adds = menuIds.Where(menuId => !hasInMenus.Any(x => x.MenuId == menuId));
            if (adds.Any())
            {
                var addInMenus = adds.Select(menuId => new RoleMenu(menuId, roleName, CurrentTenant.Id));
                await DbContext.AddRangeAsync(addInMenus, GetCancellationToken(cancellationToken));
            }
        }
    }
}
