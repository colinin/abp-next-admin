using LINGYUN.ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class EfCoreAggregateReRouteRepository : ApiGatewayEfCoreRepositoryBase<ApiGatewayDbContext, AggregateReRoute, int>, IAggregateReRouteRepository
    {
        public EfCoreAggregateReRouteRepository(IDbContextProvider<ApiGatewayDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> AggregateReRouteNameExistsAsync(string name)
        {
            return await (await GetDbSetAsync()).AnyAsync(ar => ar.Name.Equals(name));
        }

        public virtual async Task<AggregateReRoute> GetByRouteIdAsync(long routeId)
        {
            return await (await WithDetailsAsync())
                .Where(ar => ar.ReRouteId.Equals(routeId)).FirstOrDefaultAsync();
        }

        public virtual async Task<List<AggregateReRoute>> GetByAppIdAsync(string appId)
        {
            return await (await WithDetailsAsync())
                .Where(ar => ar.AppId.Equals(appId)).ToListAsync();
        }

        public virtual async Task<(List<AggregateReRoute> routes, long total)> GetPagedListAsync(string appId, string filter = "", 
            string sorting = "", int skipCount = 1, int maxResultCount = 100)
        {
            var resultReRoutes = await (await WithDetailsAsync())
                .Where(ar => ar.AppId.Equals(appId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), ar => ar.ReRouteKeys.Contains(filter) ||
                    ar.UpstreamHost.Contains(filter) || ar.UpstreamPathTemplate.Contains(filter))
                .OrderBy(ar => sorting ?? ar.Name)
                .EfPageBy(skipCount, maxResultCount)
                .ToListAsync();

            var total = await (await GetQueryableAsync())
                .Where(ar => ar.AppId.Equals(appId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), ar => ar.ReRouteKeys.Contains(filter) ||
                    ar.UpstreamHost.Contains(filter) || ar.UpstreamPathTemplate.Contains(filter)).LongCountAsync();

            return ValueTuple.Create(resultReRoutes, total);
        }
    }
}
