using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IRouteGroupRepository : IBasicRepository<RouteGroup, Guid>
    {
        Task<IEnumerable<RouteGroupAppKey>> GetActivedAppsAsync();

        Task<RouteGroup> GetByAppIdAsync(string appId);

        Task<(List<RouteGroup> Routers, long TotalCount)> GetPagedListAsync(string filter = "", string sorting = "", int skipCount = 1, int maxResultCount = 10);
    }
}
