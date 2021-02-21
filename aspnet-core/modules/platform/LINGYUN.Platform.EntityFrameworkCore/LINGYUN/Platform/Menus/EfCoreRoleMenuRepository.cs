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

        public virtual async Task<List<RoleMenu>> GetListByRoleNameAsync(string roleName, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x => x.RoleName.Equals(roleName))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task InsertAsync(IEnumerable<RoleMenu> roleMenus, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(roleMenus, GetCancellationToken(cancellationToken));
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
    }
}
