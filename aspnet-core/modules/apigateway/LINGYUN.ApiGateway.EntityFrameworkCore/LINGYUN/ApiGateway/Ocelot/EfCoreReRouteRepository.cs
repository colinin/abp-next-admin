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

        public virtual async Task<ReRoute> GetByNameAsync(string routeName)
        {
            var reRoute = await (await WithDetailsAsync()).Where(x => x.ReRouteName.Equals(routeName)).FirstOrDefaultAsync();
            return reRoute ?? throw new EntityNotFoundException(typeof(ReRoute), routeName);
        }

        public virtual async Task<ReRoute> GetByReRouteIdAsync(long routeId)
        {
            var reRoute = await (await WithDetailsAsync()).Where(x => x.ReRouteId.Equals(routeId)).FirstOrDefaultAsync();
            return reRoute ?? throw new EntityNotFoundException(typeof(ReRoute), routeId);
        }

        public virtual async Task<List<ReRoute>> GetByAppIdAsync(string appId)
        {
            return await (await WithDetailsAsync())
                .Where(r => r.AppId.Equals(appId))
                .ToListAsync();
        }

        public virtual async Task<(List<ReRoute> routes, long total)> GetPagedListAsync(string appId, string filter = "", 
            string sorting = "", int skipCount = 1, int maxResultCount = 100)
        {
            var resultReRoutes = await (await WithDetailsAsync())
                .Where(r => r.AppId.Equals(appId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.ReRouteName.Contains(filter) || 
                    r.DownstreamHostAndPorts.Contains(filter) || r.ServiceName.Contains(filter))
                .OrderBy(r => sorting ?? r.ReRouteName)
                .EfPageBy(skipCount, maxResultCount)
                .ToListAsync();

            var total = await (await GetQueryableAsync())
                .Where(r => r.AppId.Equals(appId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.ReRouteName.Contains(filter) ||
                    r.DownstreamHostAndPorts.Contains(filter) || r.ServiceName.Contains(filter))
                .LongCountAsync();

            return ValueTuple.Create(resultReRoutes, total);
        }

        public virtual async Task RemoveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var dbContext = await GetDbContextAsync();
            var entityType = dbContext.Model.FindEntityType(typeof(ReRoute));
            var tableName = entityType.GetTableName();//.Relational().TableName;

            var sqlText = $"DELETE FROM @tableName";
            var sqlParam = new List<object> { new { tableName } };

            // TODO: Test
            await dbContext.Database.ExecuteSqlRawAsync(sqlText, sqlParam, cancellationToken);
            //await DbContext.Database.ExecuteSqlCommandAsync(sqlText, sqlParam, cancellationToken);
        }

        public override async Task<IQueryable<ReRoute>> WithDetailsAsync()
        {
            return await WithDetailsAsync(
                x => x.AuthenticationOptions,
                x => x.CacheOptions,
                x => x.HttpHandlerOptions,
                x => x.LoadBalancerOptions,
                x => x.QoSOptions,
                x => x.RateLimitOptions,
                x => x.SecurityOptions);
        }

        [Obsolete("将在abp框架移除之后删除")]
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
