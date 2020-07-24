using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.Routes
{
    public class EfCoreRouteRepository : EfCoreRepository<IPlatformDbContext, Route, Guid>, IRouteRepository, ITransientDependency
    {
        public EfCoreRouteRepository(
            IDbContextProvider<IPlatformDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<Route>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Route> GetAsync(string displayName, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(
                    ou => ou.DisplayName == displayName,
                    GetCancellationToken(cancellationToken)
                );
        }

        public virtual async Task<List<Route>> GetChildrenAsync(Guid? parentId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(x => x.ParentId == parentId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Route>> GetPagedListAsync(string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .OrderBy(sorting ?? nameof(Route.Code))
                .Page(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Route>> GetRolesRouteAsync(string roleName, CancellationToken cancellationToken = default)
        {
            var roleRoutes = await (from route in DbSet
                                    join roleRoute in DbContext.Set<RoleRoute>()
                                      on route.Id equals roleRoute.RouteId
                                    where roleRoute.RoleName.Equals(roleName)
                                    select route)
                              .ToListAsync(GetCancellationToken(cancellationToken));

            return roleRoutes;
        }

        public virtual async Task<List<Route>> GetUsersRouteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var userRoutes = await (from route in DbSet
                                    join userRoute in DbContext.Set<UserRoute>()
                                      on route.Id equals userRoute.RouteId
                                    where userRoute.UserId.Equals(userId)
                                    select route)
                              .ToListAsync(GetCancellationToken(cancellationToken));

            return userRoutes;
        }

        public virtual async Task<bool> IsInRouteAsync(string roleName, Route route, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<RoleRoute>()
                .AnyAsync(x => x.RouteId.Equals(route.Id) && x.RoleName.Equals(roleName),
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> IsInRouteAsync(Guid userId, Route route, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<UserRoute>()
                   .AnyAsync(x => x.RouteId.Equals(route.Id) && x.UserId.Equals(userId),
                       GetCancellationToken(cancellationToken));
        }

        public virtual async Task RemoveAllRolesRouteAsync(Route route, CancellationToken cancellationToken = default)
        {
            var roleRoutes = await DbContext.Set<RoleRoute>()
                .Where(x => x.RouteId.Equals(route.Id))
                .Select(x => new RoleRoute(x.Id))
                .AsNoTracking()
                .ToArrayAsync(GetCancellationToken(cancellationToken));

            DbContext.Set<RoleRoute>().AttachRange(roleRoutes);
            DbContext.Set<RoleRoute>().RemoveRange(roleRoutes);
        }

        public virtual async Task RemoveAllUsersRouteAsync(Route route, CancellationToken cancellationToken = default)
        {
            var userRoutes = await DbContext.Set<UserRoute>()
                .Where(x => x.RouteId.Equals(route.Id))
                .Select(x => new UserRoute(x.Id))
                .AsNoTracking()
                .ToArrayAsync(GetCancellationToken(cancellationToken));

            DbContext.Set<UserRoute>().AttachRange(userRoutes);
            DbContext.Set<UserRoute>().RemoveRange(userRoutes);
        }

        public virtual async Task RemoveRoleRouteAsync(string roleName, Route route, CancellationToken cancellationToken = default)
        {
            var roleRoutes = await DbContext.Set<RoleRoute>()
                .Where(x => x.RouteId.Equals(route.Id) && x.RoleName.Equals(roleName))
                .Select(x => new RoleRoute(x.Id))
                .AsNoTracking()
                .ToArrayAsync(GetCancellationToken(cancellationToken));

            DbContext.Set<RoleRoute>().AttachRange(roleRoutes);
            DbContext.Set<RoleRoute>().RemoveRange(roleRoutes);
        }

        public virtual async Task RemoveUserRouteAsync(Guid userId, Route route, CancellationToken cancellationToken = default)
        {
            var userRoutes = await DbContext.Set<UserRoute>()
                .Where(x => x.RouteId.Equals(route.Id) && x.UserId.Equals(userId))
                .Select(x => new UserRoute(x.Id))
                .AsNoTracking()
                .ToArrayAsync(GetCancellationToken(cancellationToken));

            DbContext.Set<UserRoute>().AttachRange(userRoutes);
            DbContext.Set<UserRoute>().RemoveRange(userRoutes);
        }
    }
}
