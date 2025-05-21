using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.Menus;

public class EfCoreUserMenuRepository : EfCoreRepository<PlatformDbContext, UserMenu, Guid>, IUserMenuRepository
{
    public EfCoreUserMenuRepository(
        IDbContextProvider<PlatformDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<bool> UserHasInMenuAsync(
        Guid userId, 
        string menuName, 
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return await
            (from userMenu in dbContext.Set<UserMenu>()
             join menu in dbContext.Set<Menu>()
                  on userMenu.MenuId equals menu.Id
             where userMenu.UserId.Equals(userId)
             select menu)
             .AnyAsync(
                x => x.Name.Equals(menuName),
                GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<UserMenu>> GetListByUserIdAsync(
        Guid userId,
        string framework = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var menus = dbContext.Set<Menu>();
        var userMenus = dbContext.Set<UserMenu>().Where(x => x.UserId == userId);

        IQueryable<UserMenu> queryable;
        if (!framework.IsNullOrWhiteSpace())
        {
            queryable = from menu in menus
                        join userMenu in userMenus
                            on menu.Id equals userMenu.MenuId
                        where menu.Framework == framework
                        select userMenu;
        }
        else
        {
            queryable = userMenus;
        }

        return await queryable.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<Menu> FindStartupMenuAsync(
        Guid userId,
        string framework = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var userMenuQuery = dbContext.Set<UserMenu>()
            .Where(x => x.UserId.Equals(userId))
            .Where(x => x.Startup);

        return await
            (from userMenu in userMenuQuery
             join menu in dbContext.Set<Menu>()
                  on userMenu.MenuId equals menu.Id
             select menu)
             .WhereIf(!framework.IsNullOrWhiteSpace(), x => x.Framework == framework)
             .OrderByDescending(x => x.CreationTime)
             .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
