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

namespace LINGYUN.Platform.Menus;

public class EfCoreRoleMenuRepository : EfCoreRepository<PlatformDbContext, RoleMenu, Guid>, IRoleMenuRepository
{
    public EfCoreRoleMenuRepository(
        IDbContextProvider<PlatformDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<List<RoleMenu>> GetListByRoleNameAsync(
        string roleName,
        string framework = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var menus = dbContext.Set<Menu>();
        var roleMenus = dbContext.Set<RoleMenu>().Where(x => x.RoleName == roleName);

        IQueryable<RoleMenu> queryable;
        if (!framework.IsNullOrWhiteSpace())
        {
            queryable = from menu in menus
                        join roleMenu in roleMenus
                            on menu.Id equals roleMenu.MenuId
                        where menu.Framework == framework
                        select roleMenu;
        }
        else
        {
            queryable = roleMenus;
        }

        return await queryable.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<bool> RoleHasInMenuAsync(
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

    public async virtual Task<Menu> FindStartupMenuAsync(
        IEnumerable<string> roleNames,
        string framework = null,
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
             .WhereIf(!framework.IsNullOrWhiteSpace(), x => x.Framework == framework)
             .OrderByDescending(x => x.CreationTime)
             .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
