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
            return await (await GetDbSetAsync()).Where(x => x.RoleName.Equals(roleName))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> RoleHasInMenuAsync(
            string roleName, 
            string menuName, 
            CancellationToken cancellationToken = default)
        {
            var menuQuery = (await GetDbContextAsync()).Set<Menu>().Where(x => x.Name == menuName);

            return await
                (from roleMenu in (await GetDbSetAsync())
                 join menu in menuQuery
                      on roleMenu.MenuId equals menu.Id
                 select roleMenu)
                 .AnyAsync(x => x.RoleName == roleName, 
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Menu> GetStartupMenuAsync(
            IEnumerable<string> roleNames,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var roleMenuQuery = dbContext.Set<RoleMenu>()
                .Where(x => roleNames.Contains(x.RoleName))
                .Where(x => x.Startup);

            return await
                (from roleMenu in roleMenuQuery
                 join menu in dbContext.Set<Menu>()
                      on roleMenu.MenuId equals menu.Id
                 select menu)
                 .OrderByDescending(x => x.CreationTime)
                 .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }
    }
}
