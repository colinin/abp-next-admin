using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IReRouteRepository : IBasicRepository<ReRoute, int>
    {
        Task<ReRoute> GetByNameAsync(string routeName);

        Task<ReRoute> GetByReRouteIdAsync(long routeId);

        Task<List<ReRoute>> GetByAppIdAsync(string appId);

        Task<(List<ReRoute> routes, long total)> GetPagedListAsync(string appId, string filter = "", string sorting = "", int skipCount = 1, int maxResultCount = 100);

        Task DeleteAsync(Expression<Func<ReRoute, bool>> predicate, bool autoSave = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        Task RemoveAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
}
