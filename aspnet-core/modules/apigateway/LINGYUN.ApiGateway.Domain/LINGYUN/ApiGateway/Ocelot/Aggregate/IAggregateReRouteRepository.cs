using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IAggregateReRouteRepository : IBasicRepository<AggregateReRoute, int>
    {
        Task<bool> AggregateReRouteNameExistsAsync(string name);

        Task<AggregateReRoute> GetByRouteIdAsync(long routeId);

        Task<List<AggregateReRoute>> GetByAppIdAsync(string appId);

        Task<(List<AggregateReRoute> routes, long total)> GetPagedListAsync(string appId, string filter = "", string sorting = "", int skipCount = 1, int maxResultCount = 100);
    }
}
