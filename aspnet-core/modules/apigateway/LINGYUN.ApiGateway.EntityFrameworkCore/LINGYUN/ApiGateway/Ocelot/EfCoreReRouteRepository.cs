using LINGYUN.ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class EfCoreReRouteRepository : ApiGatewayEfCoreRepositoryBase<ApiGatewayDbContext, ReRoute, int>, IReRouteRepository
    {
        public EfCoreReRouteRepository(IDbContextProvider<ApiGatewayDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<ReRoute> GetByNameAsync(string routeName)
        {
            var reRoute = await WithDetails().Where(x => x.ReRouteName.Equals(routeName)).FirstOrDefaultAsync();
            return reRoute ?? throw new EntityNotFoundException(typeof(ReRoute), routeName);
        }

        public async Task<ReRoute> GetByReRouteIdAsync(long routeId)
        {
            var reRoute = await WithDetails().Where(x => x.ReRouteId.Equals(routeId)).FirstOrDefaultAsync();
            return reRoute ?? throw new EntityNotFoundException(typeof(ReRoute), routeId);
        }

        public async Task<List<ReRoute>> GetByAppIdAsync(string appId)
        {
            return await WithDetails()
                .Where(r => r.AppId.Equals(appId))
                .ToListAsync();
        }

        public async Task<(List<ReRoute> routes, long total)> GetPagedListAsync(string appId, string filter = "", 
            string sorting = "", int skipCount = 1, int maxResultCount = 100)
        {
            var resultReRoutes = await WithDetails()
                .Where(r => r.AppId.Equals(appId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.ReRouteName.Contains(filter) || 
                    r.DownstreamHostAndPorts.Contains(filter) || r.ServiceName.Contains(filter))
                .OrderBy(r => sorting ?? r.ReRouteName)
                .EfPageBy(skipCount, maxResultCount)
                .ToListAsync();

            var total = await GetQueryable()
                .Where(r => r.AppId.Equals(appId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.ReRouteName.Contains(filter) ||
                    r.DownstreamHostAndPorts.Contains(filter) || r.ServiceName.Contains(filter))
                .LongCountAsync();

            return ValueTuple.Create(resultReRoutes, total);
        }

        public async Task RemoveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entityType = DbContext.Model.FindEntityType(typeof(ReRoute));
            var tableName = entityType.GetTableName();//.Relational().TableName;

            var sqlText = $"DELETE FROM @tableName";
            var sqlParam = new List<object> { new { tableName } };

            // TODO: Test
            await DbContext.Database.ExecuteSqlRawAsync(sqlText, sqlParam, cancellationToken);
            //await DbContext.Database.ExecuteSqlCommandAsync(sqlText, sqlParam, cancellationToken);
        }

        public override IQueryable<ReRoute> WithDetails()
        {
            return WithDetails(
                x => x.AuthenticationOptions,
                x => x.CacheOptions,
                x => x.HttpHandlerOptions,
                x => x.LoadBalancerOptions,
                x => x.QoSOptions,
                x => x.RateLimitOptions,
                x => x.SecurityOptions);
        }
    }
}
