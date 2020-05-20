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

        public async Task<bool> AggregateReRouteNameExistsAsync(string name)
        {
            return await DbSet.AnyAsync(ar => ar.Name.Equals(name));
        }

        public async Task<AggregateReRoute> GetByRouteIdAsync(long routeId)
        {
            return await WithDetails().Where(ar => ar.ReRouteId.Equals(routeId)).FirstOrDefaultAsync();
        }

        public async Task<List<AggregateReRoute>> GetByAppIdAsync(string appId)
        {
            return await WithDetails().Where(ar => ar.AppId.Equals(appId)).ToListAsync();
        }

        public async Task<(List<AggregateReRoute> routes, long total)> GetPagedListAsync(string appId, string filter = "", 
            string sorting = "", int skipCount = 1, int maxResultCount = 100)
        {
            var resultReRoutes = await WithDetails()
                .Where(ar => ar.AppId.Equals(appId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), ar => ar.ReRouteKeys.Contains(filter) ||
                    ar.UpstreamHost.Contains(filter) || ar.UpstreamPathTemplate.Contains(filter))
                .OrderBy(ar => sorting ?? ar.Name)
                .EfPageBy(skipCount, maxResultCount)
                .ToListAsync();

            var total = await GetQueryable()
                .Where(ar => ar.AppId.Equals(appId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), ar => ar.ReRouteKeys.Contains(filter) ||
                    ar.UpstreamHost.Contains(filter) || ar.UpstreamPathTemplate.Contains(filter)).LongCountAsync();

            return ValueTuple.Create(resultReRoutes, total);
        }
    }
}
