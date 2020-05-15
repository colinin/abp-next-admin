using LINGYUN.ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class EfCoreRouteGroupRepository : ApiGatewayEfCoreRepositoryBase<ApiGatewayDbContext, RouteGroup, Guid>, IRouteGroupRepository
    {
        public EfCoreRouteGroupRepository(
            IDbContextProvider<ApiGatewayDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<IEnumerable<RouteGroupAppKey>> GetActivedAppsAsync()
        {
            return await DbSet.Select(g => new RouteGroupAppKey(g.AppId, g.AppName)).ToArrayAsync();
        }

        public virtual async Task<RouteGroup> GetByAppIdAsync(string appId)
        {
            var routeGroup = await DbSet.Where(g => g.AppId.Equals(appId)).FirstOrDefaultAsync();
            return routeGroup ?? throw new EntityNotFoundException(typeof(RouteGroup), appId);
        }

        public virtual async Task<(List<RouteGroup> Routers, long TotalCount)> GetPagedListAsync(string filter = "", string sorting = "", int skipCount = 1, int maxResultCount = 10)
        {
            var groups = await DbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), g => g.AppId.Contains(filter) ||
                    g.AppName.Contains(filter) || g.AppIpAddress.Contains(filter) || g.Description.Contains(filter))
                .OrderBy(g => sorting ?? nameof(RouteGroup.AppId))
                .EfPageBy(skipCount, maxResultCount)
                .ToListAsync();
            var toatl = await DbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), g => g.AppId.Contains(filter) ||
                    g.AppName.Contains(filter) || g.AppIpAddress.Contains(filter) || g.Description.Contains(filter))
                .LongCountAsync();

            return ValueTuple.Create(groups, toatl);
        }
    }
}
