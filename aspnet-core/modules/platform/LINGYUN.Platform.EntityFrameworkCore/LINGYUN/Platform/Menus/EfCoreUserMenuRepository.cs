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
    public class EfCoreUserMenuRepository : EfCoreRepository<PlatformDbContext, UserMenu, Guid>, IUserMenuRepository
    {
        public EfCoreUserMenuRepository(
            IDbContextProvider<PlatformDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> UserHasInMenuAsync(
            Guid userId, 
            string menuName, 
            CancellationToken cancellationToken = default)
        {
            return await
                (from userMenu in DbContext.Set<UserMenu>()
                 join menu in DbContext.Set<Menu>()
                      on userMenu.MenuId equals menu.Id
                 where userMenu.UserId.Equals(userId)
                 select menu)
                 .AnyAsync(
                    x => x.Name.Equals(menuName),
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<UserMenu>> GetListByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x => x.UserId.Equals(userId))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task InsertAsync(IEnumerable<UserMenu> userMenus, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(userMenus, GetCancellationToken(cancellationToken));
        }
    }
}
